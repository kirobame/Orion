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
            Repository.Register(token,value);
        }
        void OnDisable()
        {
            if (!lifetimeLinkage) return;
            Repository.Unregister(token);
        }

        void Awake()
        {
            if (lifetimeLinkage) return;
            Repository.Register(token,value);
        }
        void OnDestroy()
        {
            if (lifetimeLinkage) return;
            Repository.Unregister(token);
        }
    }
}