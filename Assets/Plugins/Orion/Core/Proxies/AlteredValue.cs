using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker]
    public class AlteredValue<T> : IReadable<T>
    {
        #region Encapsulated Types

        [HideReferenceObjectPicker]
        private class Input
        {
            [SerializeField] private int index;
            [SerializeField] private IReadable proxy;
            
            public Operand.Input ToOperandInput() => new Operand.Input(index, proxy.Read());
        }
        #endregion
        
        [ListDrawerSettings(CustomAddFunction = "AddNewInput"), SerializeField, Indent(-1)] private Input[] inputs = new Input[0];
        [SerializeField, Indent(-1)] private Operand[] operands = new Operand[0];
        
        object IReadable.Read() => Read();
        public T Read()
        {
            var operandInputs = new Operand.Input[inputs.Length];
            for (var i = 0; i < inputs.Length; i++) operandInputs[i] = inputs[i].ToOperandInput();
            
            for (var i = 0; i < operands.Length; i++) operandInputs = operands[i].Compute(operandInputs);
            var output = operandInputs.First().Value;

            return (T)output;
        }
        
        private Input AddNewInput() => new Input();
    }
}