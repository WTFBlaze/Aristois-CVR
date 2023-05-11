using System;
using UnityEngine;

namespace Aristois.Components
{
    internal class ObjectHandler : MonoBehaviour
    {
        // Credits to Obi for this class <3
        internal event Action OnStart_E;
        internal event Action OnUpdate_E;
        internal event Action OnEnable_E;
        internal event Action OnDisable_E;
        internal event Action OnDestroy_E;

        internal void Start()
            => OnStart_E?.Invoke();

        internal void Update()
            => OnUpdate_E?.Invoke();

        internal void OnEnable()
            => OnEnable_E?.Invoke();

        internal void OnDisable()
            => OnDisable_E?.Invoke();

        internal void OnDestroy()
            => OnDestroy_E?.Invoke();
    }
}
