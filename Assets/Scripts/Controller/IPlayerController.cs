using UnityEngine;

/// <summary>
/// Interface for player controller
/// </summary>
public interface IPlayerController
{
    public Vector3 Velocity { get; }
    public FrameInput FrameInputImpl { get; }
    public bool JumpingThisFrame { get; }
    public bool landingThisFrame { get; }
    public Vector3 RawMovement { get; }
    public bool Grounded { get; }
}
