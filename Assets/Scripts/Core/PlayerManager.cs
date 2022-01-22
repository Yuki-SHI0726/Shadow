using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerManager:Singleton
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool m_inWhiteRegion = true;
    [SerializeField] private float m_shadowScale = 1.0f;

    private static PlayerManager m_instance = null;

    private PlayerManager(){}

    private void Awake()
    {
        m_instance = this;
    }

    public bool IsInWhiteRegion() { return m_inWhiteRegion; }
    public void SetPlayerInWhiteRegion(bool IsInWhiteRegion) { m_inWhiteRegion = IsInWhiteRegion; }

    public float GetShadowScale() { return m_shadowScale; }
    public void SetShadowScale(float inScale) { m_shadowScale = inScale; }
}
