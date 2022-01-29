using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Should be attached to an UI object that reacts to on hover behaviors
/// </summary>
public class OnHover : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset = new Vector3();

    private void Start()
    {
        Disable();
    }

    public void AttachTo(Selectable selected)
    {
        transform.position = selected.transform.position + m_offset;
        GetComponent<Image>().enabled = true;
    }

    public void Disable()
    {
        GetComponent<Image>().enabled = false;
    }
}
