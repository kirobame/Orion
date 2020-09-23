using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Ludiq.PeekCore;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class SomeBehaviour : SerializedMonoBehaviour
{
    public OrionEvent someEvent = new OrionEvent();

    [Button]
    private void Invoke() => someEvent.Invoke();
}

public class OtherBehaviour : SerializedMonoBehaviour
{
    public void Log(Vector2 vect, string text) => Debug.Log($"vect : {text}");
}