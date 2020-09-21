using System;
using UnityEngine;

namespace Orion 
{
    [CreateAssetMenu(fileName = "NewRemoteSingleEvent", menuName = "Orion/Remote/Event/Single", order = -3000)]
    public class RemoteSingleEvent : RemoteEvent<Single> { }
}