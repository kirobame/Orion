using UnityEngine;

namespace Orion
{
    public abstract class DynamicKey<T> : IReadable<T>
    {
        [SerializeField] private Object owner;
        [SerializeField] private DynamicToken token;

        public T Get() => token.Get<T>(owner);
        public bool TryGet(out T value) => token.TryGet<T>(owner, out value);

        public void Register(T value) => token.Register(owner, value);

        object IReadable.Read() => Read();
        public T Read() => Get();
    }
}