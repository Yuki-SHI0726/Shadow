using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovablePlane : MonoBehaviour
{
    [Header("Ralative Shift")]
    [SerializeField] private Vector3 Shift;
    [SerializeField] private float Duration;
    [SerializeField] public Vector3 Velocity;

    private Vector3 m_LastPosition;
    void Start()
    {
        transform.DOMove(transform.position + Shift, Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(1);
        
        m_LastPosition = transform.position;
    }

    private void Update()
    {  
        Velocity = (transform.position - m_LastPosition) / Time.deltaTime;

        m_LastPosition = transform.position;
    }

}
