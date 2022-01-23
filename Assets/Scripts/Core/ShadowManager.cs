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
    [SerializeField] private GameObject m_ShadowRender;
    [SerializeField] private GameObject m_ShadowAnimation;
    [SerializeField] private  Vector3 OriginalShadowRenderScale;
    [SerializeField] private Animator m_ShadowAnimator;

    void Start()
    {
        m_playerObject = GameObject.FindGameObjectWithTag("Player");
        m_playerManager = m_playerObject.GetComponent<PlayerManager>();
        m_playerController = m_playerObject.GetComponent<Controller>();
        OriginalShadowRenderScale = m_ShadowRender.transform.localScale;
        m_ShadowAnimator = m_ShadowAnimation.GetComponent<Animator>();
    }

    private void Update()
    {
        if(m_isClosetoFlower && !m_HasSpawnShadow)
        {
            m_ShadowRender.SetActive(true);
        }
        else
        {
            m_ShadowRender.SetActive(false);
        }

        //影子随距离直接变化
        /*if (m_distance < 1.5f)
            m_ShadowRender.transform.localScale = OriginalShadowRenderScale * 0.5f;
        else if (m_distance >= 1.5f && m_distance <= 2.2f)
        {
            m_ShadowRender.transform.localScale = OriginalShadowRenderScale;
        }
        else
        {
            m_ShadowRender.transform.localScale = OriginalShadowRenderScale * 2.0f;
        }*/

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
                FlashBack(m_ShadowPosition);
            }
            else
            {
                //只允许在地上放影子
                //只允许在白天放影子
                if (m_playerController.Grounded && m_isClosetoFlower )
                {
                    m_HasSpawnShadow = true;


                    m_ShadowPosition = m_playerObject.transform.position;

                    //在花的范围内才能改变影子大小

                    if (m_distance < 1.5f)
                    {
                        m_playerManager.SetShadowScale(0.5f);
                    }
                    else if (m_distance >= 1.5f && m_distance <= 2.2f)
                    {
                        m_playerManager.SetShadowScale(1.0f);
                    }
                    else
                    {
                        m_playerManager.SetShadowScale(2.0f);
                    }

                    SpawnShadow(m_ShadowPosition);
                }
            }
        }
    }

    void FlashBack(Vector3 shadowPosition)
    {
        OnFlushBack.Invoke();

        m_HasSpawnShadow = false;

        m_playerObject.transform.position = shadowPosition;

        m_playerObject.transform.localScale = m_playerManager.GetOriginalScale();
        m_playerObject.transform.localScale *= m_playerManager.GetShadowScale();

        Destroy(SpawnedShadow);
    }

    void SpawnShadow(Vector3 shadowPosition)
    {
        //直接在规定的位置创建预制体
        SpawnedShadow = GameObject.Instantiate(Shadow);
        SpawnedShadow.transform.position = CalculateShadowPosition(shadowPosition);
        SpawnedShadow.transform.localScale *= (m_playerManager.GetShadowScale());
        if(m_playerObject.transform.localScale.x > 0  )
        {
            SpawnedShadow.transform.localScale = new Vector3(- SpawnedShadow.transform.localScale.x, SpawnedShadow.transform.localScale.y, SpawnedShadow.transform.localScale.z);
        }
    }

    Vector3 CalculateShadowPosition(Vector3 shadowPosition)
    {
        Vector3 Result = shadowPosition;

        if(!Mathf.Approximately(m_playerManager.GetShadowScale(),1.0f))
        {
            float Height = shadowPosition.y - m_playerManager.GetBounds().size.y / 2;
            Height += m_playerManager.GetShadowScale() * m_playerManager.GetOriginalcharacterBounds().size.y / 2;
            Result.y = Height;
        }

        return Result;
    }
    #endregion
}
