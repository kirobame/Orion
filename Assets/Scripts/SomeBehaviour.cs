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
using UnityEngine.Events;

public class SomeBehaviour : SerializedMonoBehaviour
{
    public OrionEvent someEvent = new OrionEvent();

    [Button]
    private void Invoke()
    {
        someEvent.Invoke();
    }

    public void SomeMethod(float number, string text)
    {
        
    }
    
    public void OtherMethod(float number)
    {
        
    }
}