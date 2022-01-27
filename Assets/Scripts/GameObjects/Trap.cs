using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnButtonPressed()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonReleased()
    {
        gameObject.SetActive(true);
    }
}
