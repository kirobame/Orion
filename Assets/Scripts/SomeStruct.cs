using Sirenix.OdinInspector;
using UnityEngine;

public struct SomeStruct
{
    public float Number => number;
    public string Text => text;
    
    [SerializeField] private float number;
    [SerializeField] private string text;
}