using System;
using UnityEngine;

namespace TickMono
{
    public class Tick : MonoBehaviour
    {
        public Action OnTick;
        void Update()
        {
            OnTick?.Invoke();
        }
    }
}
