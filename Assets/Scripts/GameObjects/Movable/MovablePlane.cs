using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Manage the movable plane 's transform
/// </summary>
public class MovablePlane : MonoBehaviour
{
    [Header("DoTween")]
    [SerializeField] private Vector3 m_shift = Vector3.zero;
    [SerializeField] private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private float m_duration = 0.0f;
    [SerializeField] private LoopType m_moveloopType = LoopType.Yoyo;
    [SerializeField] private int m_loops = -1; // -1 means infinite
    [SerializeField] private float m_delayTime = 1.0f;
    
    private Tweener m_tweenAnimationOrigin;
    private Tweener m_tweenAnimationHigher;
    private Vector3 m_lastPosition = Vector3.zero;
    private bool m_ToogleHigher = false;

    public Vector3 GetVelocity() { return m_velocity; }

    void Start()
    {
        m_tweenAnimationOrigin = transform.DOMove(transform.position + m_shift, m_duration)
            .SetLoops(m_loops, m_moveloopType)
            .SetDelay(m_delayTime)
            .SetAutoKill(false)
            .OnRewind(OnRewind);

        m_lastPosition = transform.position;
    }

    private void Update()
    {
        m_velocity = (transform.position - m_lastPosition) / Time.deltaTime;
        m_lastPosition = transform.position;
    }

    public void FlowerMove()
    {
        m_tweenAnimationOrigin.SmoothRewind();
    }

   public void OnRewind()
    {
        m_tweenAnimationOrigin.Restart();
    }
}
