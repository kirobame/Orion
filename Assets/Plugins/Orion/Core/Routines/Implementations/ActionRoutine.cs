using System;
using System.Collections;

namespace Orion
{
    public class ActionRoutine : Routine
    {
        public Action action;
    
        public override IEnumerator GetCall()
        {
            action();
            yield break;
        }
    }
}