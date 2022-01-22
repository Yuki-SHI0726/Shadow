using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerManager:SIngleton
/// </summary>
public class PlayerManager : MonoBehaviour
{
    //True if Player in white Region
    [SerializeField] public bool Region = true;

    private static PlayerManager m_instance = null;

    private PlayerManager(){}

    private void Awake()
    {
        m_instance = this;
    }
}
