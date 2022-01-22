using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveByButton : MonoBehaviour
{
    [SerializeField] private Vector3 Shift = Vector3.zero;
    [SerializeField] private float Duration = 2.0f;

    private Tweener m_tweenAnimation;

    private void Start()
    {
        //初始化一个Tween动画
        m_tweenAnimation = transform.DOMove(transform.position + Shift, Duration);
        m_tweenAnimation.Pause();
        m_tweenAnimation.SetAutoKill(false);
    }

    public void OnButtonPressed()
    {
        m_tweenAnimation.PlayForward();
    }

    public void OnButtonReleased()
    {
        m_tweenAnimation.PlayBackwards();
    }
}
