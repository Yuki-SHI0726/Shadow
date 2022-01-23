using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonDelayReset : Interactive
{
    [SerializeField] public UnityEvent OnButtonPressed;
    [SerializeField] public UnityEvent OnButtonRealese;
    [SerializeField] private LayerMask m_interactionLayer;
    [SerializeField] private float delayTime;

    private BoxCollider2D m_boxCollider;

    private void Start()
    {
        Type = InteractiveType.kButton;
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
            StartCoroutine(Delay(delayTime));
        }
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);

        OnButtonRealese.Invoke();
    }
}
