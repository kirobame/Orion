using UnityEngine;

namespace Orion
{
    public class Referencer : MonoBehaviour, IReferencer
    {
        public Token Token => token;
        public Object Value => value;

        [SerializeField] private Token token;
        [SerializeField] private Object value;
        [SerializeField] private bool lifetimeLinkage;

        void OnEnable()
        {
            if (!lifetimeLinkage) return;
            Repository.Register(this);
        }
        void OnDisable()
        {
            if (!lifetimeLinkage) return;
            Repository.Unregister(this);
        }

        void Awake()
        {
            if (lifetimeLinkage) return;
            Repository.Register(this);
        }
        void OnDestroy()
        {
            if (lifetimeLinkage) return;
            Repository.Unregister(this);
        }
        
        void IReferencer.SetValue(Object value)
        {
            if (!Repository.Objects.ContainsKey(token))
            {
                this.value = value;
                return;
            }
            
            Repository.Unregister(this);
            this.value = value;
            Repository.Register(this);
        }
    }
}