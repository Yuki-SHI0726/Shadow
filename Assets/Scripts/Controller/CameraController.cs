using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
   [SerializeField] public List <PolygonCollider2D> m_sceneCameras;
    [SerializeField] private CinemachineConfiner m_currentCinemachineConfiner;

    // Start is called before the first frame update
    void Start()
    {
        m_currentCinemachineConfiner = GetComponent<CinemachineConfiner>();
    }

    // Update is called once per frame
    void Update()
    {
        int Index = 0;
        if(GameManager.CameraScene == SceneCamera.First)
        {
            Index = 0;
        } else if(GameManager.CameraScene == SceneCamera.Second)
        {
            Index = 1;
        } else if(GameManager.CameraScene != SceneCamera.Third)
        {
            Index = 2;
        }

        if(m_currentCinemachineConfiner != null && m_currentCinemachineConfiner.m_BoundingShape2D != m_sceneCameras[Index])
        {
            m_currentCinemachineConfiner.m_BoundingShape2D = m_sceneCameras[Index];
        }
    }
}
