using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleport the player to other positions
/// </summary>
public class Teleporter : Interactive
{
    [SerializeField] private Vector3 m_destination = Vector3.zero;
    [SerializeField] private SceneCamera m_DestinationScene = SceneCamera.First;
    private bool m_canTeleport = false;


    public override void OnInteract(GameObject interactedObject)
    {
        m_interactedObject = interactedObject;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        m_canTeleport = true;
    }

    private void Update()
    {
        if (m_canTeleport && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Assert(m_interactedObject != null);
            m_interactedObject.transform.position = m_destination;
            GameManager.CameraScene = m_DestinationScene;
            OnExitInteract(m_interactedObject);
        }
    }

    public override void OnExitInteract(GameObject interactedObject)
    {
        m_interactedObject = interactedObject;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        m_canTeleport = false;
    }
}
