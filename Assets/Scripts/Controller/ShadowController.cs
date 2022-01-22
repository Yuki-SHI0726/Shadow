using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField] private PlayerManager m_playerManager;
    [SerializeField] private GameObject m_shadowManagerGameObejct;
    [SerializeField] private ShadowManager m_shadowManager;
    public Vector3 RawMovement { get; private set; }
    public bool Grounded => m_colDown;
    public Vector3 Velocity { get; private set; }

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
    private RayRange m_raysUp = new RayRange();
    private RayRange m_raysRight = new RayRange();
    private RayRange m_raysDown = new RayRange();
    private RayRange m_raysLeft = new RayRange();
    private bool m_colUp = false;
    private bool m_colRight = false;
    private bool m_colDown = false;
    private bool m_colLeft = false;
    private float m_timeLeftGrounded = 0.0f;
    private Bounds m_OriginalcharacterBounds;
    #endregion

    #region GravityVariables
    [Header("GRAVITY")]
    [SerializeField] private float m_fallClamp = -40f;
    [SerializeField] private float m_minFallSpeed = 80f;
    [SerializeField] private float m_maxFallSpeed = 120f;
    private float m_fallSpeed = 0.0f;
    #endregion

    #region MoveVariables
    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int m_freeColliderIterations = 100;
    [SerializeField] public Vector3 ExtensionVelocity = Vector3.zero;
    #endregion

    void Start()
    {
        m_playerManager = GetComponent<PlayerManager>();
        m_OriginalcharacterBounds = m_characterBounds;
        m_shadowManagerGameObejct = GameObject.FindGameObjectWithTag("ShadowManager");
        m_shadowManager = m_shadowManagerGameObejct.GetComponent<ShadowManager>();
    }

    void Update()
    {
        RunCollisionChecks();

        CalculateGravity();

        MoveCharacter();

        m_shadowManager.m_ShadowPosition = transform.position;
    }

    #region CollisionFunctions 

    /// <summary>
    /// Pre-collision detection using raycasts
    /// </summary>
    private void RunCollisionChecks()
    {
        CalculateRayRanged();

        bool groundedCheck = RunDetection(m_raysDown) || RunInteractivesDection(m_raysDown);

        if (m_colDown && !groundedCheck)
        {
            m_timeLeftGrounded = Time.time;
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

        m_raysDown = new RayRange(b.min.x + m_rayBuffer, b.min.y, b.max.x - m_rayBuffer, b.min.y, Vector2.down);
        m_raysUp = new RayRange(b.min.x + m_rayBuffer, b.max.y, b.max.x - m_rayBuffer, b.max.y, Vector2.up);
        m_raysLeft = new RayRange(b.min.x, b.min.y + m_rayBuffer, b.min.x, b.max.y - m_rayBuffer, Vector2.left);
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
            float fallSpeed =  m_fallSpeed;

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

            if (Physics2D.OverlapBox(posToTry, m_characterBounds.size, 0.0f, m_groundLayer))
            {
                transform.position = positionToMoveTo;

                if (i == 1)
                {
                    if(m_colDown)
                    {
                        Vector3 dir = Vector3.up;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else if(m_colLeft)
                    {
                        Vector3 dir = Vector3.right;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else if(m_colRight)
                    {
                        Vector3 dir = Vector3.left;
                        transform.position += dir.normalized * move.magnitude;
                    }
                    else if(m_colUp)
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
