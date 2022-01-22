using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class ButtonInteraction : Interactive
{
<<<<<<< HEAD
    [SerializeField] private bool m_pressed = false;
    [SerializeField] private Object m_door = null;
=======
    [SerializeField] private bool Pressed = false;
    [SerializeField] public UnityEvent OnButtonPressed;
    [SerializeField] public UnityEvent OnButtonRealese;

>>>>>>> 245f07ec45dc29d02a90f86b8786fd2b1344eb27

    private void Start()
    {
        Type = InteractiveType.kButton;
    }

    public override void OnInteract()
    {
<<<<<<< HEAD
        m_pressed = true;
    }

    public override void OnExitInteract()
    {
        m_pressed = false;
=======
        Pressed = true;
        OnButtonPressed.Invoke();
    }

    public override void OnExitInteract()
    { 
        Pressed = false;
        OnButtonRealese.Invoke();
>>>>>>> 245f07ec45dc29d02a90f86b8786fd2b1344eb27
    }
}
