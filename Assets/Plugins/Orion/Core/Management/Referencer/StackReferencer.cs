using UnityEngine;

namespace Orion
{
    public class StackReferencer : MonoBehaviour, IReferencer
    {
        public Token Token => token;
        public Object Value => value;
        
        [SerializeField] private Token token;
        [SerializeField] private Object value;
        [SerializeField] private bool lifetimeLinkage;
        
        void OnEnable()
        {
            if (!lifetimeLinkage) return;
            Repository.RegisterStack(this);
        }
        void OnDisable()
        {
            if (!lifetimeLinkage) return;
            Repository.RemoveFromStack(this);
        }

        void Awake()
        {
            if (lifetimeLinkage) return;
            Repository.RegisterStack(this);
        }
        void OnDestroy()
        {
            if (lifetimeLinkage) return;
            Repository.RemoveFromStack(this);
        }

        void IReferencer.SetValue(Object value)
        {
            if (!Repository.Stacks.ContainsKey(token) || !Repository.Stacks[token].Contains(this.value))
            {
                this.value = value;
                return;
            }
            
            Repository.RemoveFromStack(this);
            this.value = value;
            Repository.RegisterStack(this);
        }
    }
}