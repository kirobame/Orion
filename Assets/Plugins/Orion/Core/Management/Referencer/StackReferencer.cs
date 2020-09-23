using UnityEngine;

namespace Orion
{
    public class StackReferencer : ReferencerBase
    {
        protected override void Register() => Repository.RegisterStack(this);
        protected override void Unregister() => Repository.RemoveFromStack(this);
    }
}