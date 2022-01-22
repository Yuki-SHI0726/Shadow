using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField] private Vector3 m_shadowPosition = Vector3.zero;

    void Start()
    {
        Debug.Log("Spawn a shadow");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
