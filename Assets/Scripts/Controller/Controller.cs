using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controller 
{
    public struct FrameInput
    {
        public float X, Y;
        public bool JumpDown;
        public bool JumpUp;
        public bool Shadow;
    }

    public interface IPlayerController
    {
        public Vector3 Velocity { get; }
        public FrameInput Input { get; }
        public bool JumpingThisFrame { get; }
        public bool LandingThisFrame { get; }
        public Vector3 RawMovement { get; }
        public bool Grounded { get; }
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
    public class Controller : MonoBehaviour, IPlayerController
    {
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => m_colDown;

        private Vector3 m_lastPosition;
        private float m_currentHorizontalSpeed;
        private float m_currentVerticalSpeed;

        private bool m_active;
        void Awake() => Invoke(nameof(Activate), 0.5f);
        void Activate() => m_active = true;

        private void Update()
        {
            if (!m_active) return;

            GatherInput();

            DetectFlashBack();

            RunCollisionChecks();

            //计算速度
            CalculateWalk();
            CalculateJumpApex();
            CalculateGravity();
            CalculateJump();

            //真正的移动
            MoveCharacter();
        }


        #region Gather Input
        private void GatherInput()
        {
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Shadow = UnityEngine.Input.GetButtonDown("Shadow")
            };

            //如果按下了跳跃,记录跳跃的时间
            if (Input.JumpDown) {
                m_lastJumpPressed = Time.time;
            }
        }
        #endregion

        #region Collisions

        [Header("COLLISION")] 
        [SerializeField] private Bounds m_characterBounds;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private LayerMask m_flowerLayer;
        [SerializeField] public string MovableTag;
        [SerializeField] private int m_detectorCount = 3;
        [SerializeField] private float m_detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float m_rayBuffer = 0.1f;

        //m_raysX代表X方向上的物理检测
        private RayRange m_raysUp, m_raysRight, m_raysDown, m_raysLeft;
        private bool m_colUp, m_colRight, m_colDown, m_colLeft;

        private float m_timeLeftGrounded;

        // 用Raycast来做预碰撞信息
        private void RunCollisionChecks() 
        {
            // 计算射线的长度和方向 
            CalculateRayRanged();

            // 当前帧是不是在Ground上
            LandingThisFrame = false;
            bool groundedCheck = RunDetection(m_raysDown) || RunFlowerDection(m_raysDown);
            if (m_colDown && !groundedCheck)// 只会在离地的那一帧触发
            { 
                m_timeLeftGrounded = Time.time;
            } 
            else if (!m_colDown && groundedCheck)
            {
                m_coyoteUsable = true;
                LandingThisFrame = true;
            }

            m_colDown = groundedCheck;

            // 剩下的信息
            m_colUp = RunDetection(m_raysUp);
            m_colLeft = RunDetection(m_raysLeft);
            m_colRight = RunDetection(m_raysRight);

            bool RunFlowerDection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, m_detectionRayLength, m_flowerLayer));
            }

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, m_detectionRayLength, m_groundLayer));
            }
        }

        private void CalculateRayRanged() {

            //建立一个以Player原点为中心的包围盒
            Bounds b = new Bounds(transform.position, m_characterBounds.size);

            m_raysDown = new RayRange(b.min.x + m_rayBuffer, b.min.y, b.max.x - m_rayBuffer, b.min.y, Vector2.down);
            m_raysUp = new RayRange(b.min.x + m_rayBuffer, b.max.y, b.max.x - m_rayBuffer, b.max.y, Vector2.up);
            m_raysLeft = new RayRange(b.min.x, b.min.y + m_rayBuffer, b.min.x, b.max.y - m_rayBuffer, Vector2.left);
            m_raysRight = new RayRange(b.max.x, b.min.y + m_rayBuffer, b.max.x, b.max.y - m_rayBuffer, Vector2.right);
        }

        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < m_detectorCount; i++) 
            {
                var t = (float)i / (m_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        //Debug
        private void OnDrawGizmos() {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + m_characterBounds.center, m_characterBounds.size);

            // Rays
            if (!Application.isPlaying) {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { m_raysUp, m_raysRight, m_raysDown, m_raysLeft }) 
                {
                    foreach (var point in EvaluateRayPositions(range)) 
                    {
                        Gizmos.DrawRay(point, range.Dir * m_detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying)
            {
                return;
            }

            // 将移动的位置给画出来
            Gizmos.color = Color.red;
            var move = new Vector3(m_currentHorizontalSpeed, m_currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + move, m_characterBounds.size);
        }

        #endregion

        #region Walk

        [Header("WALKING")] 
        [SerializeField] private float m_acceleration = 90;
        [SerializeField] private float m_moveClamp = 13;
        [SerializeField] private float m_deAcceleration = 60f;
        [SerializeField] private float m_apexBonus = 2;

        private void CalculateWalk()
        {
            if (Input.X != 0) 
            {
                // 设置横向移动
                m_currentHorizontalSpeed += Input.X * m_acceleration * Time.deltaTime;

                // 最大速度限制
                m_currentHorizontalSpeed = Mathf.Clamp(m_currentHorizontalSpeed, -m_moveClamp, m_moveClamp);

                // 
                var apexBonus = Mathf.Sign(Input.X) * m_apexBonus * m_apexPoint;
                m_currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else 
            {
                // 如果没有输入的话,计算当前速度
                m_currentHorizontalSpeed = Mathf.MoveTowards(m_currentHorizontalSpeed, 0, m_deAcceleration * Time.deltaTime);
            }

            // 如果有碰撞,则不运动
            if (m_currentHorizontalSpeed > 0 && m_colRight || m_currentHorizontalSpeed < 0 && m_colLeft)
            {
                m_currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] 
        [SerializeField] private float m_fallClamp = -40f;
        [SerializeField] private float m_minFallSpeed = 80f;
        [SerializeField] private float m_maxFallSpeed = 120f;
        private float m_fallSpeed;

        private void CalculateGravity() 
        {
            if (m_colDown) 
            {
                if(m_currentVerticalSpeed < 0)
                    m_currentVerticalSpeed = 0;
            }
            else 
            {
                // 如果我们提前停止跳跃,在向上的时候施加向下的力
                var fallSpeed = m_endedJumpEarly && m_currentVerticalSpeed > 0 ? m_fallSpeed * m_jumpEndEarlyGravityModifier : m_fallSpeed;

                // 下落
                m_currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp下落速度
                if (m_currentVerticalSpeed < m_fallClamp) 
                    m_currentVerticalSpeed = m_fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")]
        [SerializeField] private float m_jumpHeight = 30;
        [SerializeField] private float m_jumpApexThreshold = 10f;
        [SerializeField] private float m_coyoteTimeThreshold = 0.1f;
        [SerializeField] private float m_jumpBuffer = 0.1f;
        [SerializeField] private float m_jumpEndEarlyGravityModifier = 3;
        private bool m_coyoteUsable;
        private bool m_endedJumpEarly = true;
        private float m_apexPoint; // Becomes 1 at the apex of a jump
        private float m_lastJumpPressed;
        private bool CanUseCoyote => m_coyoteUsable && !m_colDown && m_timeLeftGrounded + m_coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => m_colDown && m_lastJumpPressed + m_jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!m_colDown)
            {
                m_apexPoint = Mathf.InverseLerp(m_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                m_fallSpeed = Mathf.Lerp(m_minFallSpeed, m_maxFallSpeed, m_apexPoint);
            }
            else
            {
                m_apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            //只有在地面,在Buffer中,在Coyote中可以跳跃
            if (Input.JumpDown && CanUseCoyote || HasBufferedJump) {
                m_currentVerticalSpeed = m_jumpHeight;
                m_endedJumpEarly = false;
                m_coyoteUsable = false;
                m_timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else {
                JumpingThisFrame = false;
            }

            // 提前结束碰撞
            if (!m_colDown && Input.JumpUp && !m_endedJumpEarly && Velocity.y > 0)
            {
                m_endedJumpEarly = true;
            }

            //如果碰撞到了上面
            if (m_colUp) {
                if (m_currentVerticalSpeed > 0) m_currentVerticalSpeed = 0;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")] 
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int m_freeColliderIterations = 100;
        [SerializeField] public Vector3 ExtensionVelocity;

        //我们先设置边界,以免碰撞发生
        private void MoveCharacter()
        {
            Vector3 pos = transform.position;
            RawMovement = new Vector3(m_currentHorizontalSpeed, m_currentVerticalSpeed); // Used externally
            Vector3 Movement = RawMovement + ExtensionVelocity;

            Vector3 move = Movement * Time.deltaTime;
            Vector3 furthestPoint = pos + move;
       
            Collider2D hit = Physics2D.OverlapBox(furthestPoint, m_characterBounds.size, 0, m_groundLayer);

            if (!hit) 
            {
                transform.position += move;
                return;
            }

            Vector3 positionToMoveTo = transform.position;
            for (int i = 1; i < m_freeColliderIterations; i++)
            {
                float t = (float)i / m_freeColliderIterations;
                Vector2 posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, m_characterBounds.size, 0, m_groundLayer))
                {
                    transform.position = positionToMoveTo;

                    if (i == 1)
                    {
                        if (m_currentVerticalSpeed < 0) m_currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }
        #endregion

        #region FlashBack
        [Header("FlashBack")]
        [SerializeField] public bool m_HasSpawnShadow = false;
        [SerializeField] public Vector3 m_ShadowPosition;
        [SerializeField] public GameObject Shadow;
        [SerializeField] public GameObject SpawnedShadow;
        void DetectFlashBack()
        {
            if(Input.Shadow)
            {
                if(m_HasSpawnShadow)
                {
                    m_HasSpawnShadow = false;
                    FlashBack(m_ShadowPosition);
                    Destroy(SpawnedShadow);                 
                }
                else
                {
                    //只允许在地上放影子
                    if(Grounded)
                    {
                        m_HasSpawnShadow = true;
                        m_ShadowPosition = transform.position;
                        SpawnShadow(m_ShadowPosition);               
                    }
                }
            }
        }

        void FlashBack(Vector3 shadowPosition)
        {
            transform.position = shadowPosition;
        }

        void SpawnShadow(Vector3 shadowPosition)
        {
            //直接在规定的位置创建预制体
            SpawnedShadow = GameObject.Instantiate(Shadow);
            SpawnedShadow.transform.position = shadowPosition;
        }

        #endregion
    }
}