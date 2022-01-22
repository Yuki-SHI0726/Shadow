using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class ButtonInteraction : Interactive
{
    [SerializeField] private bool Pressed = false;
    [SerializeField] public UnityEvent OnButtonPressed;
    [SerializeField] public UnityEvent OnButtonRealese;

    private void Start()
    {
        Type = InteractiveType.kButton;
    }

    public override void OnInteract()
    {
        Pressed = true;
        OnButtonPressed.Invoke();
    }

    public override void OnExitInteract()
    {
        Pressed = false;
        OnButtonRealese.Invoke();
    }
}
