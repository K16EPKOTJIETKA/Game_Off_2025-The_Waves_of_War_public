using System;
using System.Collections.Generic;
using Injection;
using TickMono;
namespace SequenceRunner
{
    public class CoroutineSequenceRunner : IDisposable
    {
        [Inject] private  Injector _injector;
        [Inject] private Tick _tick;
        public void Initialize()
        {
            _tick.OnTick += Update;
        }
        public void Dispose()
        {
            _tick.OnTick -= Update;
        }
        // Dense storage for fast iteration
        private readonly List<CoroutineBasedSequenceBase> _sequences = new List<CoroutineBasedSequenceBase>();
        // Parallel dense list of IDs to avoid requiring Sequence to expose Id
        private readonly List<int> _ids = new List<int>();
        // O(1) lookup from id -> index in the dense lists
        private readonly Dictionary<int, int> _idToIndex = new Dictionary<int, int>();

        // ID generator (wraps and skips used ids)
        private int _nextSequenceId = int.MinValue;

        public int RunSequence(CoroutineBasedSequenceBase sequence, Action<bool> onSuccessfullyFinished, params object[] parameters)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            _injector.Inject(sequence);

            // Find a free id (handle wrap and full table)
            int start = _nextSequenceId;
            int id = start;
            while (_idToIndex.ContainsKey(id))
            {
                id = (id == int.MaxValue) ? int.MinValue : id + 1;
                if (id == start)
                    throw new InvalidOperationException("No free sequence ids available.");
            }
            _nextSequenceId = (id == int.MaxValue) ? int.MinValue : id + 1;

            // Initialize and register
            sequence.RunSequence(id, onSuccessfullyFinished, parameters);

            int index = _sequences.Count;
            _sequences.Add(sequence);
            _ids.Add(id);
            _idToIndex[id] = index;

            return id;
        }

        public bool CancelSequence(int sequenceId)
        {
            if (!_idToIndex.TryGetValue(sequenceId, out int i))
                return false;

            // Cancel then swap-remove
            _sequences[i].Cancel();
            SwapRemoveAt(i);
            return true;
        }

        public void CancelAll()
        {
            // Cancel without allocating intermediate lists
            for (int i = _sequences.Count - 1; i >= 0; i--)
            {
                _sequences[i].Cancel();
            }
            _sequences.Clear();
            _ids.Clear();
            _idToIndex.Clear();
        }

        private void Update()
        {
            // One pass: update and swap-remove finished items (order not preserved)
            int i = 0;
            while (i < _sequences.Count)
            {
                var seq = _sequences[i];

                if (seq.IsReadyToFinish)
                {
                    seq.End();
                    SwapRemoveAt(i);   // do not ++i; re-check element that was swapped in
                }
                else
                {
                    seq.Update();
                    i++;
                }
            }
        }

        private void OnDisable() => CancelAll();
        private void OnDestroy() => CancelAll();

        /// <summary>
        /// Swap-remove element at index i from the dense lists; updates _idToIndex.
        /// </summary>
        private void SwapRemoveAt(int i)
        {
            int last = _sequences.Count - 1;
            int removedId = _ids[i];

            if (i != last)
            {
                // Move last into hole
                _sequences[i] = _sequences[last];
                _ids[i] = _ids[last];

                // Fix index for moved id
                _idToIndex[_ids[i]] = i;
            }

            // Pop tail
            _sequences.RemoveAt(last);
            _ids.RemoveAt(last);
            _idToIndex.Remove(removedId);
        }
    }
}