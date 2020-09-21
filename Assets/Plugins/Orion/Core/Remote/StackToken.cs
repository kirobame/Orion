using UnityEngine;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewStackToken", menuName = "Orion/Tokens/Stack")]
    public class StackToken : Token
    {
        public override object Read() => Repository.Stacks[this];
    }
}