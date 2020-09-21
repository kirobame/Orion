using UnityEngine;

namespace Orion 
{
    [CreateAssetMenu(fileName = "NewRemoteVector3Event", menuName = "Orion/Remote/Event/Vector3", order = -3000)]
    public class RemoteVector3Event : RemoteEvent<Vector3> { }
}