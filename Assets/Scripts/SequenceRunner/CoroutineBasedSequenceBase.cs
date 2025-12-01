using System;

namespace SequenceRunner
{
    public abstract class CoroutineBasedSequenceBase
    {
        private bool _readyToFinish;
        private Action<bool> _onSuccessfullyFinished; // true if sequence finished successfully, false if it was cancelled

        public abstract void Start(params object[] parameters);
        public virtual void Update() { }
        protected abstract void Finish();
        public bool IsReadyToFinish => _readyToFinish;
        public int SequenceId { get; private set; }
        protected void MarkReadyToFinish()
        {
            _readyToFinish = true;
        }
        public void RunSequence(int sequenceId, Action<bool> onSuccessfullyFinished, params object[] parameters)
        {
            _readyToFinish = false;
            _onSuccessfullyFinished = onSuccessfullyFinished;
            SequenceId = sequenceId;
            Start(parameters);
        }

        public void Cancel()
        {
            Finish();
            _onSuccessfullyFinished?.Invoke(false);
        }

        public void End()
        {
            Finish();
            _onSuccessfullyFinished?.Invoke(true);
        }
    }
}