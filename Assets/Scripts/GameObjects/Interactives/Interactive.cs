using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive: MonoBehaviour
{
    protected GameObject m_interactedObject = null;

    public InteractiveType Type { get; protected set; }

    public string Name { get; protected set; }

    public virtual void OnInteract(GameObject interactedObject) 
    {
        m_interactedObject = interactedObject;
    }

    public virtual void OnExitInteract(GameObject interactedObject)
    {
        m_interactedObject = null;
    }

    public virtual void OnStayInteract(GameObject interactedObject) { }
}
