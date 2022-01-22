using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : Interactive
{
    [SerializeField] private bool Pressed = false;
    [SerializeField] private Object  Door = null;
    private void Start()
    {
        Type = InteractiveType.kButton;
    }

    public override void OnInteract()
    {
        Pressed = true;
    }

    public override void OnExitInteract()
    { 
        Pressed = false;
    }
}
