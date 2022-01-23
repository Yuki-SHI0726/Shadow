using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void Start()
    {
        // Set respawn point invisible
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
