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
    [SerializeField] private Bounds m_bounds = new Bounds(new Vector3(-54.0102005f, 55.4232864f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f));
    [SerializeField] private Bounds m_originalcharacterBounds = new Bounds();
    [SerializeField] private Vector3 m_originalScale = Vector3.one;

    public bool IsInWhiteRegion() { return m_inWhiteRegion; }
    public void SetPlayerInWhiteRegion(bool IsInWhiteRegion) { m_inWhiteRegion = IsInWhiteRegion; }

    public float GetShadowScale() { return m_shadowScale; }
    public void SetShadowScale(float inScale) { m_shadowScale = inScale; }

    public Bounds GetBounds() { return m_bounds; }
    public void SetBounds(Bounds InBounds) { m_bounds = InBounds; }

    public Bounds GetOriginalcharacterBounds() { return m_originalcharacterBounds; }

    public void SetOriginalcharacterBounds(Bounds InBounds) { m_originalcharacterBounds = InBounds; }

    public Vector3 GetOriginalScale() { return m_originalScale; }

    public void SetOriginalScale(Vector3 InScale) { m_originalScale=InScale; }
}
