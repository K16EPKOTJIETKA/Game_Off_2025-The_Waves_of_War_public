using UnityEngine;
using Injection;
using SequenceRunner;

namespace Controllers
{
    public class DemodulatorController : MonoBehaviour
    {
        [SerializeField] private GameObject demodulatorGraph;

        [Inject] private CoroutineSequenceRunner _sequenceRunner;

        public void Initialize()
        {
            //initialization here
        }

        public void PlayerInteraction()
        {
            _sequenceRunner.RunSequence(new DemodulatorSequence(), (success) => {
                if (success)
                {
                    Debug.Log("Demodulator sequence finished successfully");
                }
            });
        }
    }
}