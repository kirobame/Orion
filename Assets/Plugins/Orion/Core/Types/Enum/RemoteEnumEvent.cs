using System;
using UnityEngine;

namespace Orion 
{
    [CreateAssetMenu(fileName = "NewRemoteEnumEvent", menuName = "Orion/Remote/Event/Enum", order = -3000)]
    public class RemoteEnumEvent : RemoteEvent<Enum> { }
}