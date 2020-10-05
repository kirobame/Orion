using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Orion
{
    public abstract class AnimatorProxy
    {
        #if UNITY_EDITOR
        [SerializeField] protected RuntimeAnimatorController controller;
        #endif

        public abstract void Affect(Animator animator);
    }
}

