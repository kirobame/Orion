﻿using System;
using System.Collections;

namespace Orion
{
    public class IndefiniteRoutine : Routine
    {
        public Action action;
        public object waitInstruction;

        public override IEnumerator GetCall()
        {
            while (true)
            {
                action();
                yield return waitInstruction;
            }
        }
    }
}