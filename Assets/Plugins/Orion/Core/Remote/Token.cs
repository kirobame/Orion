using UnityEngine;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewToken", menuName = "Orion/Tokens/Standard")]
    public class Token : ScriptableObject, IReadable
    {
        public virtual object Read() => Repository.Objects[this];
    }
}