using Sirenix.OdinInspector;

namespace Orion
{
    [HideReferenceObjectPicker]
    public interface IProvider
    {
        object GetInstance();
    }
    
    [HideReferenceObjectPicker]
    public interface IProvider<out T> : IProvider
    {
        T GetInstance();
    }
}