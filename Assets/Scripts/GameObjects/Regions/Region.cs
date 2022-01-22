using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private bool m_isRegion = true;

    public bool IsRegion() { return m_isRegion; }
}
