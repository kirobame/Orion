using Sirenix.OdinInspector;
using UnityEngine;

[InlineProperty]
public class SomeClass : ISomeInterface
{
    public string Text => text;
    
    [SerializeField, HideLabel] private string text;
}