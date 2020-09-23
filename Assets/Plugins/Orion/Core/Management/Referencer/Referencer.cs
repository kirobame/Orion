namespace Orion
{
    public class Referencer : ReferencerBase
    {
        protected override void Register() => Repository.Register(this);
        protected override void Unregister() => Repository.Unregister(this);
    }
}