using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public abstract class Operand: ScriptableObject
    {
        #region Encpasulated Types

        public class Input
        {
            public Input(int index, object value)
            {
                Index = index;
                Value = value;
            }

            public int Index { get; private set; }
            public object Value { get; private set; }
        }
        #endregion
        
        [SerializeField] private int[] outputtedIndices;

        private readonly Dictionary<int, object> dynamicValues = new Dictionary<int, object>();
        
        public Input[] Compute(Input[] inputs)
        {
            dynamicValues.Clear();
            foreach (var input in inputs) dynamicValues.Add(input.Index, input.Value);

            var outputs = Compute(dynamicValues);

            var sentInputs = new Input[outputs.Length];
            for (var i = 0; i < outputs.Length; i++) sentInputs[i] = new Input(outputtedIndices[i], outputs[i]);

            return sentInputs;
        }
        protected abstract object[] Compute(Dictionary<int, object> dynamicValues);
    }
}