using UnityEngine;
using System;
using System.Collections;
namespace ActionCoroutine
{
    public class SimpleActionCoroutine : MonoBehaviour
    {
        /// <summary>
        /// UpdateMethod with argument 0-1 for action completion 
        /// </summary>
        public void RunSimpleLerpCoroutine(float duration, Action<float> UpdateMethod, Action EndMethod)
        {
            StartCoroutine(Enumerator(duration, UpdateMethod, EndMethod));
        }
        private IEnumerator Enumerator(float duration, Action<float> UpdateMethod, Action EndMethod)
        {
            if (duration <= 0)
            {
                EndMethod?.Invoke();
                yield break;
            }
            float currentDuration = 0;
            while (currentDuration < duration)
            {
                UpdateMethod?.Invoke(currentDuration / duration);
                currentDuration += Time.deltaTime;
                yield return null;
            }
            EndMethod?.Invoke();
            yield break;
        }
    }
}