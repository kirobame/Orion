using Orion;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRelay", menuName = "Custom/Relay")]
public class SomeRelay : ScriptableObject
{
    public void Create(IProxy<Light> proxy, IProvider<Light> provider)
    {
        var instance = provider.GetInstance();
        proxy.Write(instance);
    }

    public void Log(Light light)
    {
        Debug.Log($"{light.name} / {light.GetInstanceID()}");
    }
}