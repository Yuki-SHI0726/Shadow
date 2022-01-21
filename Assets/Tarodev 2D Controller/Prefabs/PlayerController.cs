using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ElevenController
{
    public struct FrameInput
    {
        public float X, Y;
        public bool JumpDown;
        public bool JumpUp;
        public bool Shadow;
    }

    public struct RayRange
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
        {
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
            Dir = dir;
        }

        public readonly Vector2 Start, End, Dir;
    }

    public class PlayerController : MonoBehaviour
    {
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }

        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }

        private Rigidbody2D m_PlayerRigidBody;
        private SpriteRenderer m_PlayerSpriteRender;
        private BoxCollider2D m_PlayerCollier2D;

        [Space]
        [Header("Velocity")]
        public float RunVelocity;
        public float JumpVelocity;

        public float NomalJumpForce = 66f;
        public float AdditionalJumpForce = 22f;
        public float AdditionalJumpTime = 0.1f;

        [Space]
        [Header("State")]
        public bool BottomCollider;
        public bool OnGround;
        public bool OnJumpFlag;
        public bool isAdditionalJump = false;
        public bool hasDashed = false;
        public bool Mobable = true;
        public bool JumpPressed = false;

        void Start()
        {
            m_PlayerRigidBody = GetComponent<Rigidbody2D>();
            m_PlayerSpriteRender = GetComponent<SpriteRenderer>();
            m_PlayerCollier2D = GetComponent<BoxCollider2D>();
        }

        private void GatherInput()
        {
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Shadow = UnityEngine.Input.GetButtonDown("Shadow")
            };
        }

        void Update()
        {
            GatherInput();

            OnGround = BottomCollider;

            OnJumpFlag = OnGround && Input.JumpUp;

            Flip(Input.X);

            Run(Input.X, Input.Y);

            if (OnJumpFlag)
            {
                Jump();
            }

            if (isAdditionalJump && JumpPressed)
            {
                m_PlayerRigidBody.AddForce(Vector2.up * AdditionalJumpForce);
            }

        }

        void Flip(float X)
        {
            if (Mathf.Abs(X) > 0.1f)
            {
                if (m_PlayerRigidBody.velocity.x >= 0.0f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }
        void Run(float X, float Y)
        {
            m_PlayerRigidBody.velocity = new Vector2(X * RunVelocity, m_PlayerRigidBody.velocity.y);
        }

        void Jump()
        {
            StartCoroutine(AddtionalJumpTimer());
            m_PlayerRigidBody.AddForce(Vector2.up * NomalJumpForce);
        }

        IEnumerator AddtionalJumpTimer()
        {
            isAdditionalJump = true;
            yield return new WaitForSeconds(AdditionalJumpTime);
            isAdditionalJump = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

        }

    }
}
