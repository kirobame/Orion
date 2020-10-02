using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class Feedback : SerializedMonoBehaviour
    {
        public event Action onCompletion;

        public virtual void Prepare() { }
        
        public abstract IEnumerator GetRoutine();
        public virtual bool GetNextIndex(int currentIndex, IReadOnlyList<Feedback> registry, out int nextIndex)
        {
            nextIndex = currentIndex + 1;
            return nextIndex < registry.Count;
        }

        protected void Complete() => onCompletion.Invoke();
    }
}