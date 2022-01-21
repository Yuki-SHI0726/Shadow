using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovablePlane : MonoBehaviour
{
    [Header("Ralative Shift")]
    [SerializeField] private Vector3 Shift;
    [SerializeField] private float Duration;
    void Start()
    {
        transform.DOMove(transform.position + Shift, Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(1);
    }

}
