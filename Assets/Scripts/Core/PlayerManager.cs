using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerManager:SIngleton
/// </summary>
public class PlayerManager : MonoBehaviour
{
    private bool m_inWhiteRegion = true;       // Yes

    private static PlayerManager m_instance = null;

    private PlayerManager(){}

    private void Awake()
    {
        m_instance = this;
    }

    public bool IsInWhiteRegion() { return m_inWhiteRegion; }
    public void SetPlayerInWhiteRegion(bool IsInWhiteRegion) { m_inWhiteRegion = IsInWhiteRegion; }
}
