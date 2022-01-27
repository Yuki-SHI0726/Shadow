using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class ButtonInteraction : Interactive
{
    [SerializeField] public UnityEvent OnButtonPressed;
    [SerializeField] public UnityEvent OnButtonRealese;
    [SerializeField] private LayerMask m_interactionLayer;

    private BoxCollider2D m_boxCollider;

    private void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void OnInteract(GameObject interactedObject)
    {
        OnButtonPressed.Invoke();
    }

    public override void OnExitInteract(GameObject interactedObject)
    {
        //要检测这个按钮上还没有没有InteractionManagerLayer的碰撞
        Collider2D HitInfo = Physics2D.OverlapBox(transform.position, m_boxCollider.size, 0, m_interactionLayer);
        if (HitInfo == null)
        {
            OnButtonRealese.Invoke();
        }
    }

    
}
