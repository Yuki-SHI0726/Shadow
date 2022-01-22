using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveByButton : MonoBehaviour
{
    [SerializeField] private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private Vector3 Shift = Vector3.zero;
    [SerializeField] private float Duration = 2.0f;

    private Tweener m_tweenAnimation;

    private Vector3 m_lastPosition = Vector3.zero;

    private void Start()
    {
        m_lastPosition = transform.position;

        //初始化一个Tween动画
        m_tweenAnimation = transform.DOMove(transform.position + Shift, Duration);
        m_tweenAnimation.Pause();
        m_tweenAnimation.SetAutoKill(false);
    }
    private void Update()
    {
        m_velocity = (transform.position - m_lastPosition) / Time.deltaTime;
        m_lastPosition = transform.position;
    }

    public void OnButtonPressed()
    {
        m_tweenAnimation.PlayForward();
    }

    public void OnButtonReleased()
    {
        m_tweenAnimation.PlayBackwards();
    }

    public Vector3 GetVelocity() { return m_velocity; }
}
