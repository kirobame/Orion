using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class ReferencerBase : MonoBehaviour, IReferencer
    {
        public Token Token => token;
        public Object Value => value;

        [SerializeField] private Token token;
        [SerializeField] private Object value;
        [SerializeField] private bool lifetimeLinkage;
        
        void OnEnable()
        {
            if (!lifetimeLinkage) return;
            Register();
        }
        void OnDisable()
        {
            if (!lifetimeLinkage) return;
            Unregister();
        }

        void Awake()
        {
            if (lifetimeLinkage) return;
            Register();
        }
        void OnDestroy()
        {
            if (lifetimeLinkage) return;
            Unregister();
        }

        protected abstract void Register();
        protected abstract void Unregister();

        void IReferencer.SetValue(Object value) => this.value = value;
    }
}