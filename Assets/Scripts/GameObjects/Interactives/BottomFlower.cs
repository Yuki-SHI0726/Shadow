using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomFlower : Interactive
{
    public UnityEvent OnCollision;
    private void Start()
    {
        Type = InteractiveType.kFlower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("log");
        OnCollision.Invoke();
    }
}
