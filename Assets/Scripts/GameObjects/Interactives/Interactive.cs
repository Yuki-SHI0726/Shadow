using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive: MonoBehaviour
{
    public InteractiveType Type{get;protected set;}

    public string Name { get; protected set; }

    public virtual void OnInteract() { }
    public virtual void OnExitInteract() { }
    public virtual void OnStayInteract() { }
}
