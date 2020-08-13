using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public bool enabled;
    public bool stunned;

    public float speedMultiplier;

    public bool overrideGravity;
    public Vector2 gravityDirection;
    public float gravityScale;

    public float scaleMultiplier;
    public float jumpHeightMultiplier;
    public float jumpTimeMultiplier;

    /// <summary>
    /// Drives both scale and jump
    /// </summary>
    public float chonkMultiplier;

    public PlayerStats()
    {
        enabled = true;
        stunned = false;
        speedMultiplier = 1f;
        overrideGravity = true;
        gravityDirection = Vector2.down;
        gravityScale = 9.81f;
        scaleMultiplier = 1f;
        jumpHeightMultiplier = 1f;
        jumpTimeMultiplier = 1f;
        chonkMultiplier = 1f;
    }
}
