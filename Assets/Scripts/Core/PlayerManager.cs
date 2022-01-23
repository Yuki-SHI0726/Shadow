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
    [SerializeField] private Bounds m_bounds;
    [SerializeField] private Bounds m_OriginalcharacterBounds;
    [SerializeField] private Vector3 m_OriginalScale = Vector3.one;

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

    public Bounds GetBounds() { return m_bounds; }
    public void SetBounds(Bounds InBounds) { m_bounds = InBounds; }

    public Bounds GetOriginalcharacterBounds() { return m_OriginalcharacterBounds; }

    public void SetOriginalcharacterBounds(Bounds InBounds) { m_OriginalcharacterBounds = InBounds; }

    public Vector3 GetOriginalScale() { return m_OriginalScale; }

    public void SetOriginalScale(Vector3 InScale) { m_OriginalScale=InScale; }
}
