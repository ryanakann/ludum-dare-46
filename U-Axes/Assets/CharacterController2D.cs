using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    #region STATS
    public class StatModifiers
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

        public StatModifiers() {
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
    #endregion

    #region EXPOSED VARIABLES
    [HideInInspector]
    public StatModifiers stats;

    [SerializeField] 
    [Range(0.1f, 20f)]
    private float baseSpeed = 1f;

    [SerializeField]
    [Range(0f, 50f)]
    private float baseJumpForce = 1f;

    [SerializeField]
    [Range(0f, 10f)]
    private float fallMultiplier = 3f;
    
    [SerializeField]
    [Range(0f, 10f)]
    private float lowJumpMultiplier = 8f;

    [SerializeField]
    private LayerMask groundMask;
    #endregion


    #region LOCAL VARIABLES
    private bool grounded;
    private Vector2 groundedCheckCenter;
    private Vector2 groundedCheckSize;

    private Vector2 gravity;
    private Vector2 gravityLF;
    private float gravityScale;
    private Vector3 transformUpSmoothing;

    private float y;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private Vector2 up;
    private Vector2 right;
    private Vector2 upVelocity;
    private Vector2 rightVelocity;
    private Vector2 targetVelocity;
    
    private Vector3 baseLocalScale;
    private Vector3 localScaleSmoothing;
    #endregion


    #region REFERENCES
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    #endregion


    private void Awake()
    {
        stats = new StatModifiers();
        
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        grounded = false;

        gravity = stats.overrideGravity ? stats.gravityDirection.normalized * stats.gravityScale : Physics2D.gravity;
        gravityLF = gravity;

        baseLocalScale = transform.localScale;
    }


    /// <summary>
    /// Moves the player. Must be called from FixedUpdate every frame.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="jumpDown"></param>
    /// <param name="jumpStay"></param>
    public void Move(float x, bool jumpDown, bool jumpStay)
    {
        //Scale
        transform.localScale = Vector3.SmoothDamp(transform.localScale, baseLocalScale * stats.chonkMultiplier * stats.scaleMultiplier, ref localScaleSmoothing, 0.01f);

        //Gravity and Rotation
        gravity = stats.overrideGravity ? stats.gravityDirection.normalized * stats.gravityScale : Physics2D.gravity;
        gravityScale = gravity.magnitude;
        if (Vector2.Angle(gravity, gravityLF) < 1f)
        {
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = false;
            transform.up = Vector3.SmoothDamp(transform.up, -gravity.normalized, ref transformUpSmoothing, 0.01f);
            transform.forward = Vector3.forward;
        }
        up = transform.up;
        right = transform.right;
        gravityLF = gravity;

        bool falling = Vector2.Dot(rb.velocity, up) < 0;
        upVelocity = Vector3.Project(rb.velocity, up);
        rightVelocity = right * (x * baseSpeed * stats.speedMultiplier);
        //print($"right: {right} - x: {x} - baseSpeed: {baseSpeed} - speedMultiplier: {stats.speedMultiplier}");
        if (jumpDown)
        {
            
            upVelocity = up * (baseJumpForce * stats.jumpHeightMultiplier);
        } 
        else
        {
            if (falling)
            {
                upVelocity += gravity * fallMultiplier * Time.fixedDeltaTime;
            }
            else if (!jumpStay)
            {
                upVelocity += gravity * lowJumpMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                upVelocity += gravity * Time.fixedDeltaTime;
            }
        }
        velocity = rightVelocity + upVelocity;
        print(velocity);
        rb.velocity = velocity;
    }
}
