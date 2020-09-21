using System;
using UnityEngine;

namespace Orion 
{
    [CreateAssetMenu(fileName = "NewRemoteEnum", menuName = "Orion/Remote/Enum/Standard")]
    public class RemoteEnum : RemoteValue<Enum> { }
}