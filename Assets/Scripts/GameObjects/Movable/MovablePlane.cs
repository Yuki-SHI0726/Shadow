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
    [SerializeField] private Vector3 Shift = Vector3.zero;
    [SerializeField] public Vector3 Velocity = Vector3.zero;
    [SerializeField] private float Duration = 0.0f;
    [SerializeField] private LoopType MoveloopType = LoopType.Yoyo;
    [SerializeField] private int Loops = -1; // -1 means infinite
    [SerializeField] private int DelayTime = 1;

    private Vector3 m_LastPosition;
    void Start()
    {
        transform.DOMove(transform.position + Shift, Duration)
            .SetLoops(Loops, MoveloopType)
            .SetDelay(DelayTime);
        
        m_LastPosition = transform.position;
    }

    private void Update()
    {  
        Velocity = (transform.position - m_LastPosition) / Time.deltaTime;

        m_LastPosition = transform.position;
    }

}
