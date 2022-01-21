using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive: MonoBehaviour
{
    public InteractiveType Type{get;protected set;}

    public string Name { get; protected set; }

    public abstract void OnInteract();
}
