using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShadowManager : MonoBehaviour
{
    [SerializeField] private GameObject m_playerObject;
    [SerializeField] private PlayerManager m_playerManager;
    [SerializeField] private Controller m_playerController;
    [SerializeField] private string m_flowerTag;
    [SerializeField] private float m_distance;
    [SerializeField] private bool m_isClosetoFlower;

    void Start()
    {
        m_playerObject = GameObject.FindGameObjectWithTag("Player");
        m_playerManager = m_playerObject.GetComponent<PlayerManager>();
        m_playerController = m_playerObject.GetComponent<Controller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == m_flowerTag)
        {
            m_isClosetoFlower = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == m_flowerTag)
        {
            Vector3 PlayerToFlower = collision.transform.position - m_playerObject.transform.position;
            m_distance = PlayerToFlower.magnitude;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == m_flowerTag)
        {
            m_isClosetoFlower = false;
        }
    }

    #region FlashBack
    [Header("FlashBack")]
    [SerializeField] public bool m_HasSpawnShadow = false;
    [SerializeField] public Vector3 m_ShadowPosition;
    [SerializeField] public GameObject Shadow;
    [SerializeField] public GameObject SpawnedShadow;

    public UnityEvent OnFlushBack;
    public void DetectFlashBack(FrameInput input)
    {     
        if (input.Shadow)
        {
            if (m_HasSpawnShadow)
            {
                m_HasSpawnShadow = false;

                FlashBack(m_ShadowPosition);
                Destroy(SpawnedShadow);

                m_playerObject.transform.localScale = Vector3.one;
                m_playerObject.transform.localScale *= m_playerManager.GetShadowScale();
            }
            else
            {
                //只允许在地上放影子
                //只允许在白天放影子
                if (m_playerController.Grounded && m_playerManager.IsInWhiteRegion())
                {
                    m_HasSpawnShadow = true;

                    m_ShadowPosition = m_playerObject.transform.position;

                    //在花的范围内才能改变影子大小
                    if(m_isClosetoFlower)
                    {
                        m_playerManager.SetShadowScale(m_distance);
                    }
                    SpawnShadow(m_ShadowPosition);
                }
            }
        }
    }

    void FlashBack(Vector3 shadowPosition)
    {
        OnFlushBack.Invoke();
        m_playerObject.transform.position = shadowPosition;
    }

    void SpawnShadow(Vector3 shadowPosition)
    {
        //直接在规定的位置创建预制体
        SpawnedShadow = GameObject.Instantiate(Shadow);
        SpawnedShadow.transform.position = shadowPosition;
        SpawnedShadow.transform.localScale *= m_playerManager.GetShadowScale();
    }
    #endregion
}
