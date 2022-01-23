using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 100.0f;
    [SerializeField] private float m_currentHealth = 100.0f;

    private void Start()
    {
        Debug.Assert(m_currentHealth <= m_maxHealth);
        Debug.Assert(m_currentHealth >= 0);
    }

    public void Damage(float amount)
    {
        m_currentHealth -= amount;
    }
}
