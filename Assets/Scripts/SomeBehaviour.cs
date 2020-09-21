using System;
using System.Collections;
using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class SomeBehaviour : SerializedMonoBehaviour
{
    public OrionEvent otherEvent = new OrionEvent();

    [Button]
    private void Invoke() => otherEvent.Invoke();
}