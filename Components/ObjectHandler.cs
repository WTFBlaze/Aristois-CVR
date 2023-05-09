using System;
using UnityEngine;

namespace Aristois.Components
{
    internal class ObjectHandler : MonoBehaviour
    {
        // Credits to Obi for this class <3
        public event Action OnStart_E;
        public event Action OnUpdate_E;
        public event Action OnEnable_E;
        public event Action OnDisable_E;
        public event Action OnDestroy_E;

        public void Start()
            => OnStart_E?.Invoke();

        public void Update()
            => OnUpdate_E?.Invoke();

        public void OnEnable()
            => OnEnable_E?.Invoke();

        public void OnDisable()
            => OnDisable_E?.Invoke();

        public void OnDestroy()
            => OnDestroy_E?.Invoke();
    }
}
