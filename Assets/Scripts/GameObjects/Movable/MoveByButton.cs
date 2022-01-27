using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveByButton : MonoBehaviour
{
    [SerializeField] private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private Vector3 m_shift = Vector3.zero;
    [SerializeField] private float m_duration = 2.0f;

    private Tweener m_tweenAnimation;
    private Vector3 m_lastPosition = Vector3.zero;

    private void Start()
    {
        m_lastPosition = transform.position;
        
        // Initialize a tween animation
        m_tweenAnimation = transform.DOMove(transform.position + m_shift, m_duration);
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
