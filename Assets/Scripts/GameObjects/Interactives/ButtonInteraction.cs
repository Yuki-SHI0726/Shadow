using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : Interactive
{
    [SerializeField] private bool m_pressed = false;
    [SerializeField] private Object m_door = null;

    private void Start()
    {
        Type = InteractiveType.kButton;
    }

    public override void OnInteract()
    {
        m_pressed = true;
    }

    public override void OnExitInteract()
    {
        m_pressed = false;
    }
}
