using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class ButtonInteraction : Interactive
{
    [SerializeField] public UnityEvent OnButtonPressed;
    [SerializeField] public UnityEvent OnButtonRealese;

    private void Start()
    {
        Type = InteractiveType.kButton;
    }

    public override void OnInteract()
    {
        OnButtonPressed.Invoke();
    }

    public override void OnExitInteract()
    {
        OnButtonRealese.Invoke();
    }
}
