using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevenController
{
    public class BottomCollider : MonoBehaviour
    {
        public GameObject PlayerGameObject;
        public PlayerController PC;

        void Start()
        {
            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            PC = PlayerGameObject.GetComponent<PlayerController>();
        }

        void OnTriggerEnter2D(Collider2D Other)
        {
            if (Other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                PC.BottomCollider = true;
                PC.hasDashed = false;
            }
        }
        void OnTriggerExit2D(Collider2D Other)
        {
            if (Other.gameObject.layer == LayerMask.NameToLayer("Ground"))
                PC.BottomCollider = false;
        }
    }
}
