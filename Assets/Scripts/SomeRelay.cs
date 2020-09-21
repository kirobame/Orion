using Orion;
using UnityEngine;

[CreateAssetMenu(fileName = "SomeRelay", menuName = "Custom/Relay")]
public class SomeRelay : ScriptableObject
{
    public void Create(MeshRenderer prefab, Object owner, DynamicToken token)
    {
        var instance = Instantiate(prefab);
        token.Register(owner, instance);
    }

    public void SetColor(Object owner, Color color, DynamicToken token)
    {
        var meshRenderer = token.Get<MeshRenderer>(owner);
        meshRenderer.material.color = color;
    }
}