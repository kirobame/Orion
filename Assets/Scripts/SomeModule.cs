using Orion;
using UnityEngine;

public class SomeModule : Module<bool>
{
    protected override void OnActionStarted(bool input)
    {
        Debug.Log("STARTED");
    }

    protected override void OnAction(bool input)
    {
        Debug.Log("DURING");
    }

    protected override void OnActionEnded(bool input)
    {
        Debug.Log("ENDED");
    }
}