using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public new bool enabled = true;
    public bool stunned = false;

    [Range(0f, 20f)]
    public float speedMultiplier = 1f;

    public bool overrideGravity = false;
    public Vector2 gravityDirection = Vector2.down;
    [Range(0f, 20f)]
    public float gravityScale = 9.81f;

    [Range(0f, 20f)]
    public float scaleMultiplier = 1f;
    [Range(0f, 20f)]
    public float jumpHeightMultiplier = 1f;

    /// <summary>
    /// Drives both scale and jump
    /// </summary>
    [Range(0f, 20f)]
    public float chonkMultiplier = 1f;

    private void FixedUpdate()
    {
        gravityDirection.Normalize();
    }
}
