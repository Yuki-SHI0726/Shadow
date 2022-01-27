using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomFlower : Interactive
{
    public UnityEvent OnCollision;

    public override void OnInteract(GameObject interactedObject)
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision.Invoke();
    }
}
