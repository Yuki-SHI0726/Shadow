using UnityEngine;

/// <summary>
/// Input variable for each frame
/// </summary>
public class FrameInput : MonoBehaviour
{
    public float X, Y;
    public bool JumpDown;
    public bool JumpUp;
    public bool Shadow;

    /// <summary>
    /// Gather the input at this frame
    /// </summary>
    /// <returns>Jump time if we jumped</returns>
    public float GatherInput()
    {
        JumpDown = Input.GetButtonDown("Jump");
        JumpUp = Input.GetButtonUp("Jump");
        X = Input.GetAxisRaw("Horizontal");
        Shadow = Input.GetButtonDown("Shadow");

        // If we jumped, return the jump time
        if (JumpDown)
        {
            return Time.time;
        }

        return 0.0f;
    }
}