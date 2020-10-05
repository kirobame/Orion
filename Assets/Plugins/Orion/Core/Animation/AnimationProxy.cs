using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public class AnimationProxy : AnimatorProxy
    {
        #if UNITY_EDITOR

        private IEnumerable GetAnimations()
        {
            var list = new ValueDropdownList<int>();
            if (controller == null)
            {
                list.Add("Null", 0);
                return list;
            }

            var editorController = controller as AnimatorController;
            foreach (var layer in editorController.layers)
            {
                foreach (var stateWrapper in layer.stateMachine.states) list.Add(stateWrapper.state.name, stateWrapper.state.nameHash);
            }
            
            return list;
        }
        #endif
        
        [ValueDropdown("GetAnimations")]
        [SerializeField] private int animationId;

        public override void Affect(Animator animator) => animator.Play(animationId);
    }
}