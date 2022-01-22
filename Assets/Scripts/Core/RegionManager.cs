using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    [SerializeField] private GameObject m_playerObject = null;
    [SerializeField] private string m_regionTag = "";
    [SerializeField] private PlayerManager m_playeManager = null;

    void Start()
    {
        m_playerObject = GameObject.FindGameObjectWithTag("Player");
        m_playeManager = m_playerObject.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == m_regionTag)
        {
            Region region = collision.GetComponent<Region>(); 
            if (region != null) 
            {
                SetRegion(region.IsRegion());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    void SetRegion(bool Region)
    {
        m_playeManager.SetPlayerInWhiteRegion(Region);
    }
}
