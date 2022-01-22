using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flower : Interactive
{   
    private void Start()
    {
        Type = InteractiveType.kFlower;
    }

    public override void OnInteract(GameObject interactedObject)
    {
    }
}
