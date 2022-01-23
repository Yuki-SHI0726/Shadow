using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controller for Hardy
/// </summary>
public class Controller : MonoBehaviour, IPlayerController
{
    #region Properties
    public Vector3 Velocity { get; set; }
    public FrameInput FrameInputImpl { get; private set; }
    public bool JumpingThisFrame { get; private set; }
    public bool landingThisFrame { get; private set; }
    public Vector3 RawMovement { get; private set; }
    public bool Grounded => m_colDown;

    [SerializeField] private PlayerManager m_playerManager;
    [SerializeField] private ShadowManager m_shadowManager;
    #endregion

    #region ControllerVariables
    [SerializeField] private float m_delayInvokeTime = 0.5f;
    private Vector3 m_lastPosition = Vector3.zero;
    private float m_currentHorizontalSpeed = 0.0f;
    private float m_currentVerticalSpeed = 0.0f;
    private bool m_active = false;
    #endregion

    #region CollisionVariables
    [Header("COLLISION")]
    [SerializeField] private Bounds m_characterBounds;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_interactiveLayer;
    [SerializeField] public string MovableTag;
    [SerializeField] private int m_detectorCount = 3;
    [SerializeField] private float m_detectionRayLength = 0.1f;
    [SerializeField] [Range(0.1f, 0.3f)] private float m_rayBuffer = 0.1f;
    private RayRange m_raysUp    = new RayRange();
    private RayRange m_raysRight = new RayRange();
    private RayRange m_raysDown  = new RayRange();
    private RayRange m_raysLeft  = new RayRange();
    private bool m_colUp    = false;
    private bool m_colRight = false;
    private bool m_colDown  = false;
    private bool m_colLeft  = false;
    private float m_timeLeftGrounded = 0.0f;
    private Bounds m_OriginalcharacterBounds;
    #endregion

    #region WalkVariables
    [Header("WALKING")]
    [SerializeField] private float m_acceleration = 90;
    [SerializeField] private float m_moveClamp = 13;
    [SerializeField] private float m_deAcceleration = 60f;
    [SerializeField] private float m_apexBonus = 2;
    #endregion

    #region GravityVariables
    [Header("GRAVITY")]
    [SerializeField] private float m_fallClamp = -40f;
    [SerializeField] private float m_minFallSpeed = 80f;
    [SerializeField] private float m_maxFallSpeed = 120f;
    private float m_fallSpeed = 0.0f;
    #endregion

    #region JumpVariables
    [Header("JUMPING")]
    [SerializeField] private float m_jumpHeight = 30;
    [SerializeField] private float m_jumpApexThreshold = 10f;
    [SerializeField] private float m_coyoteTimeThreshold = 0.1f;
    [SerializeField] private float m_jumpBuffer = 0.1f;
    [SerializeField] private float m_jumpEndEarlyGravityModifier = 3;
    private bool m_coyoteUsable = false;
    private bool m_endedJumpEarly = true;
    private float m_apexPoint = 0.0f; // Becomes 1 at the apex of a jump
    private float m_lastJumpPressed = 0.0f;
    private bool HasBufferedJump
    {
        get
        {
            bool goodTime = (m_lastJumpPressed + m_jumpBuffer) > Time.time;
            return m_colDown && goodTime;
        }
    }
    private bool CanUseCoyote 
    {
        get 
        {
            bool hasEnoughTime = (m_timeLeftGrounded + m_coyoteTimeThreshold) > Time.time;
            return m_coyoteUsable && !m_colDown && hasEnoughTime; 
        }
    }
    #endregion

    #region MoveVariables
    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int m_freeColliderIterations = 100;
    [SerializeField] public Vector3 ExtensionVelocity = Vector3.zero;
    #endregion

    void Awake()
    {
        Invoke(nameof(Activate), m_delayInvokeTime);
        FrameInputImpl = new FrameInput();
        Invoke(nameof(Activate), m_delayInvokeTime);
        m_playerManager = GetComponent<PlayerManager>();
        m_OriginalcharacterBounds = m_characterBounds;
        m_playerManager.SetOriginalcharacterBounds(m_OriginalcharacterBounds);
        m_playerManager.SetOriginalScale(transform.localScale);
        m_playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_active)
        {
            return;
        }

        m_lastJumpPressed = FrameInputImpl.GatherInput();

        RunCollisionChecks();

        //ShadowManager
        m_shadowManager.DetectFlashBack(FrameInputImpl);

        // Calculation for movement
        CalculateWalk();
        CalculateJumpApex();
        CalculateGravity();
        CalculateJump();

        // Actual movement
        MoveCharacter();
        Run();
        Flip(FrameInputImpl.X);

    }

    public void Activate() { m_active = true; }
    public void Deactive() { m_active = false;}
    #region Animation

    [SerializeField] private Animator m_playerAnimator;

    void Flip(float X)
    {
        if (Mathf.Abs(X) > 0.1f)
        {
            if (m_currentHorizontalSpeed >= 0.0f)
            {
                transform.localScale = new Vector3 ( Mathf.Abs( transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(-transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void Run()
    {
        m_playerAnimator.SetFloat("X", Mathf.Abs(FrameInputImpl.X));
    }

    #endregion

    #region CollisionFunctions 

    /// <summary>
    /// Pre-collision detection using raycasts
    /// </summary>
    private void RunCollisionChecks()
    {
        CalculateRayRanged();

        bool groundedCheck = RunDetection(m_raysDown);

        m_playerAnimator.SetBool("isOnGround",groundedCheck);

        if (m_colDown && !groundedCheck)
        {
            m_timeLeftGrounded = Time.time;
        }
        else if (!m_colDown && groundedCheck)
        {
            m_coyoteUsable = true;
        }

        m_colDown = groundedCheck;
        m_colUp = RunDetection(m_raysUp);
        m_colLeft = RunDetection(m_raysLeft);
        m_colRight = RunDetection(m_raysRight);

        bool RunInteractivesDection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, m_detectionRayLength, m_interactiveLayer));
        }

        bool RunDetection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, m_detectionRayLength, m_groundLayer));
        }
    }

    public void CalculateCharacterBounds()
    {
        m_characterBounds = m_OriginalcharacterBounds;
        m_characterBounds.size *= m_playerManager.GetShadowScale();
    }
    private void CalculateRayRanged()
    {
        // Build a bounds centered by player
        Bounds b = new Bounds(transform.position, m_characterBounds.size);

        m_playerManager.SetBounds(b);

        m_raysDown  = new RayRange(b.min.x + m_rayBuffer, b.min.y, b.max.x - m_rayBuffer, b.min.y, Vector2.down);
        m_raysUp    = new RayRange(b.min.x + m_rayBuffer, b.max.y, b.max.x - m_rayBuffer, b.max.y, Vector2.up);
        m_raysLeft  = new RayRange(b.min.x, b.min.y + m_rayBuffer, b.min.x, b.max.y - m_rayBuffer, Vector2.left);
        m_raysRight = new RayRange(b.max.x, b.min.y + m_rayBuffer, b.max.x, b.max.y - m_rayBuffer, Vector2.right);
    }

    private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
    {
        for (int i = 0; i < m_detectorCount; ++i)
        {
            float t = (float)i / (m_detectorCount - 1);
            yield return Vector2.Lerp(range.Start, range.End, t);
        }
    }

    // Debug
    private void OnDrawGizmos()
    {
        // Bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + m_characterBounds.center, m_characterBounds.size);

        // Rays
        if (!Application.isPlaying)
        {
            CalculateRayRanged();
            Gizmos.color = Color.blue;
            foreach (RayRange range in new List<RayRange> { m_raysUp, m_raysRight, m_raysDown, m_raysLeft })
            {
                foreach (Vector2 point in EvaluateRayPositions(range))
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
        Vector3 move = new Vector3(m_currentHorizontalSpeed, m_currentVerticalSpeed) * Time.deltaTime;
        Gizmos.DrawWireCube(transform.position + move, m_characterBounds.size);
    }

    #endregion

    #region WalkFunctions

    private void CalculateWalk()
    {
        if (FrameInputImpl.X != 0)
        {
            // Set horizontal speed
            m_currentHorizontalSpeed += FrameInputImpl.X * m_acceleration * Time.deltaTime;

            // Limit the maximum speed
            m_currentHorizontalSpeed = Mathf.Clamp(m_currentHorizontalSpeed, -m_moveClamp, m_moveClamp);

            float apexBonus = Mathf.Sign(FrameInputImpl.X) * m_apexBonus * m_apexPoint;
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

    #region GravityFunctions

    private void CalculateGravity()
    {
        if (m_colDown)
        {
            if (m_currentVerticalSpeed < 0)
            { 
                m_currentVerticalSpeed = 0;
            }
        }
        else
        {
            // 如果我们提前停止跳跃,在向上的时候施加向下的力
            float fallSpeed = m_endedJumpEarly && m_currentVerticalSpeed > 0 ? m_fallSpeed * m_jumpEndEarlyGravityModifier : m_fallSpeed;

            // 下落
            m_currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            // Clamp下落速度
            if (m_currentVerticalSpeed < m_fallClamp)
            {
                m_currentVerticalSpeed = m_fallClamp;
            }
        }
    }

    #endregion

    #region JumpFunctions

    private void CalculateJumpApex()
    {
        if (!m_colDown)
        {
            m_apexPoint = Mathf.InverseLerp(m_jumpApexThreshold, 0.0f, Mathf.Abs(Velocity.y));
            m_fallSpeed = Mathf.Lerp(m_minFallSpeed, m_maxFallSpeed, m_apexPoint);
        }
        else
        {
            m_apexPoint = 0;
        }
    }

    private void CalculateJump()
    {
        // Only allow coyote to jump when on the ground, or in buffer
        if (FrameInputImpl.JumpDown && CanUseCoyote || HasBufferedJump)
        {
            m_currentVerticalSpeed = m_jumpHeight;
            m_endedJumpEarly = false;
            m_coyoteUsable = false;
            m_timeLeftGrounded = float.MinValue;
            JumpingThisFrame = true;
            m_playerAnimator.SetTrigger("Jump");
        }
        else
        {
            JumpingThisFrame = false;
        }

        // End collision earlier
        if (!m_colDown && FrameInputImpl.JumpUp && !m_endedJumpEarly && Velocity.y > 0.0f)
        {
            m_endedJumpEarly = true;
        }

        // If hit top
        if (m_colUp && m_currentVerticalSpeed > 0)
        {
            m_currentVerticalSpeed = 0;
        }
    }

    #endregion

    #region MoveFunctions
    /// <summary>
    /// Set bounds in advance to omit collison occurring
    /// </summary>
    private void MoveCharacter()
    {
        Vector3 pos = transform.position;
        RawMovement = new Vector3(m_currentHorizontalSpeed, m_currentVerticalSpeed); // Used externally
        Vector3 movement = RawMovement + ExtensionVelocity;

        Vector3 move = movement * Time.deltaTime;
        Vector3 furthestPoint = pos + move;

        Collider2D hit = Physics2D.OverlapBox(furthestPoint, m_characterBounds.size, 0, m_groundLayer);

        if (!hit)
        {
            transform.position += move;
            return;
        }

        Vector3 positionToMoveTo = transform.position;
        for (int i = 1; i < m_freeColliderIterations; ++i)
        {
            float t = (float)i / m_freeColliderIterations;
            Vector2 posToTry = Vector2.Lerp(pos, furthestPoint, t);

            var hitInfo = Physics2D.OverlapBox(posToTry, m_characterBounds.size, 0.0f, m_groundLayer);
            if (hitInfo)
            {
                transform.position = positionToMoveTo;

                if (i == 1)
                {
                    if (m_colDown)
                    {
                        Vector3 dir = Vector3.up;
                        transform.position += dir.normalized * move.magnitude;
                        var Movetable = hitInfo.gameObject.GetComponent<MovablePlane>();
                        if(Movetable)
                        {
                            Vector3 AttendVelocity = Movetable.GetVelocity();
                            Vector3 AttendMotion = AttendVelocity * Time.deltaTime;
                            transform.position += AttendMotion;
                        }

                    }
                    else if (m_colLeft)
                    {
                        Vector3 dir = Vector3.right;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else if (m_colRight)
                    {
                        Vector3 dir = Vector3.left;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else if (m_colUp)
                    {
                        Vector3 dir = Vector3.down;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else
                    {
                        Vector3 dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }
                }

                return;
            }

            positionToMoveTo = posToTry;
        }
    }
    #endregion

}
