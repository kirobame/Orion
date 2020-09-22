using System.Collections;
using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

public class SomeBehaviour : SerializedMonoBehaviour
{
    [SerializeField] private OrionEvent someEvent = new OrionEvent();

    public void Log(SomeStruct someStruct, float number, GameObject obj)
    {
        Debug.Log($"{someStruct.Number} / {someStruct.Text} || {number} || {obj.name}");
    }
}