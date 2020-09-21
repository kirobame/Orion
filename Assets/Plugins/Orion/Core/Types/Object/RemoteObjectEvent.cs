using UnityEngine;

namespace Orion 
{
    [CreateAssetMenu(fileName = "NewRemoteObjectEvent", menuName = "Orion/Remote/Event/Object")]
    public class RemoteObjectEvent : RemoteEvent<Object> { }
}