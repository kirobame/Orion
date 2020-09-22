using System.Collections;

namespace Orion
{
    public class InstructionRoutine : Routine
    {
        public object instruction;

        public override IEnumerator GetCall() { yield return instruction; }
    }
}