using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactive
{
    // Start is called before the first frame update
    private void Start()
    {
        Type = InteractiveType.kFlower;
    }

    public override void OnInteract()
    {
        Debug.Log("花");
    }
}
