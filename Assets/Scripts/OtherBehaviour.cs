using Sirenix.OdinInspector;
using UnityEngine;

public class OtherBehaviour : SerializedMonoBehaviour
{
    public void Log(Vector2 vect, string text) => Debug.Log($"{vect} : {text}");
}