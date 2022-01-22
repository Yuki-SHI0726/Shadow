using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] string RegionTag;
    [SerializeField] private PlayerManager m_PlayeManager;
    // Update is called once per frame
    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        m_PlayeManager = PlayerObject.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == RegionTag)
        {
            Region region = collision.transform.GetComponent<Region>();
            if(region != null)
            {
                SetRegion(region.region);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    void SetRegion(bool Region)
    {
        m_PlayeManager.Region = Region;
    }
}
