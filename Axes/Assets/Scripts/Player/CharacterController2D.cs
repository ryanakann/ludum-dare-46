using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : Entity
{
    [Header("Movement")]
    [SerializeField] public float maxJumpHeight = 1f;

    [Tooltip("How snappy a short hop is compared to a full jump")]
    [SerializeField] public float lowJumpMultiplier = 4f;
    [Tooltip("How quickly you fall after the apex of your jump")]
    [SerializeField] public float fallMultiplier = 2f;
    [Tooltip("Multiplier independent of above jump parameters")]
    [SerializeField] public float gravityMultiplier = 1f;
    private float gravityJumpMultiplier = 1f;
	[SerializeField] public float baseMoveSpeed = 2f;
    private Timer jumpCD = new Timer(0.05f);
 
	[Range(0, 0.3f)] [SerializeField] private float movementSmoothing = 0.05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask groundLayer = 1;							// A mask determining what is ground to the character

    [Header("References")]
    [SerializeField]
    public PlayerStats stats;
	[SerializeField] 
    private Transform groundCheck = null;							// A position marking where to check if the player is grounded.
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;

    const float groundedRadius = .3f; // Radius of the overlap circle to determine if grounded

    [Header("SFX")]
    public float footstepFrequencyModifier = 1f;
    private float footstepAnimationTime;
    private float footstepNextThreshold;

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;


    #region LOCAL VARIABLES
    
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Vector3 right;

    private Vector3 gravVelocity;
    private Vector2 gravity;
    private Vector2 gravityLF;

    private Vector3 baseLocalScale;
    private Vector3 localScaleSmoothing;
    private Vector3 targetLocalScale;

    private float move; private bool jumpDown; private bool jumpStay;
    private bool groundedLF = false;
    private bool facingRight;
    
    #endregion

    protected override void Awake() {
		base.Awake();

        //Component References
        if (stats == null) stats = GetComponent<PlayerStats>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
		if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();

        //Events
        if (OnLandEvent == null) OnLandEvent = new UnityEvent();

        //Jump
        jumpCD.Reset();
		
        //Gravity
        gravity = Physics2D.gravity;
		gravityLF = gravity;
        rb.gravityScale = 0f;

        //Footsteps
        footstepAnimationTime = 0f;
        footstepNextThreshold = footstepFrequencyModifier;

        //Scale
        baseLocalScale = transform.localScale;
	}

	protected override void FixedUpdate() {
		bool wasGrounded = grounded;
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger) {
				grounded = true;
				if (!wasGrounded) OnLandEvent.Invoke();
			}
		}
		anim.SetBool("grounded", grounded);

        if (stats.enabled) {
            rb.constraints = RigidbodyConstraints2D.None;

            if (!stats.stunned)
            {
                HandleMovement();
            }
        } else {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
	}


	public void Move(float move, bool jumpDown, bool jumpStay) {
        if (stats.stunned) {
            this.move = 0f;
            this.jumpDown = false;
            this.jumpStay = false;
        } else {
            this.move = move;
            this.jumpDown = jumpDown;
            this.jumpStay = jumpStay;
        }
	}

    void HandleMovement() {
        anim.SetFloat("x", move);
        velocity = rb.velocity;

        //CHECK GRAVITY
        /******************************************************************/
        gravity = stats.overrideGravity ? stats.gravityDirection * stats.gravityScale : Physics2D.gravity;

        //Only hard update transform if a certain threshold has passed to prevent jank
        if ((gravityLF - gravity).sqrMagnitude > 0.001f) {
            //Always face away from gravity
            rb.freezeRotation = false;
            transform.forward = Vector3.forward;
            transform.up = -gravity;
            rb.freezeRotation = true;
        } else {
            rb.freezeRotation = true; //When not rotating, this should always be true
        }
        gravityLF = gravity; // Record gravity of last frame, as this is the last time gravity is checked in this function

        //JUMP MODIFIER
        /******************************************************************/
        // Moving upwards, and not holding jump
        if (!grounded && !jumpStay && Vector2.Dot(gravity, velocity) < 0f) {
            gravityJumpMultiplier = lowJumpMultiplier;
        } 
        // Falling
        else if (!grounded && Vector2.Dot(gravity, velocity) > 0f) {
            gravityJumpMultiplier = fallMultiplier;
        } 
        // Grounded
        else {
            gravityJumpMultiplier = 1f;
        }
        gravity *= gravityJumpMultiplier * gravityMultiplier / stats.chonkMultiplier;

        //PLAYER CONTROL
        /******************************************************************/
        //Only allow movement if grounded, or airborne when airControl is turned on
        if (grounded || airControl) {
            // Calculate velocity caused by gravity
            gravVelocity = Vector3.Project(velocity, gravity);

            right = transform.right;
            targetVelocity = gravVelocity + right * move * baseMoveSpeed * stats.speedMultiplier * stats.chonkMultiplier; //Velocity of player = velocity due to gravity plus orthogonal player input movement

            // Subtly smooth velocity to prevent jerkiness.
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref velocity, movementSmoothing);

            //FOOTSTEPS
            /******************************************************************/
            float xSpeed = Vector3.Project(velocity, transform.right).magnitude;
            if (footstepAnimationTime > footstepNextThreshold) {
                footstepNextThreshold += footstepFrequencyModifier;
                if (FX_Spawner.instance)
                    FX_Spawner.instance.SpawnFX(FXType.FOOTSTEP, groundCheck.position, Vector3.zero);
            }
            if (grounded) {
                footstepAnimationTime += xSpeed * Time.deltaTime;
            }

            anim.SetFloat("speed", xSpeed);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight) {
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight) {
                Flip();
            }
        }

        //JUMP CONTROL
        /******************************************************************/
        if (grounded && jumpDown && jumpCD.Check()) {
            grounded = false;

            velocity = right * move * baseMoveSpeed * stats.speedMultiplier * stats.chonkMultiplier; // Zero out jump velocity
            float jumpVelocity = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * gravityMultiplier * maxJumpHeight * stats.jumpHeightMultiplier);

            velocity -= (Vector3)gravity.normalized * jumpVelocity;

            anim.SetTrigger("jump");
        }

        //Apply gravity
        velocity += (Vector3)gravity * Time.fixedDeltaTime;

        //Finally, set velocity
        rb.velocity = velocity;
    }

    private void LateUpdate()
    {
        transform.localScale = Vector3.Scale(baseLocalScale * stats.scaleMultiplier * stats.chonkMultiplier, new Vector3(facingRight ? 1 : -1, 1f, 1f));
        //transform.localScale = Vector3.SmoothDamp(transform.localScale * (facingRight ? 1 : -1), targetLocalScale, ref localScaleSmoothing, movementSmoothing);
    }

    public void Die () {
		anim.SetTrigger("hurt");
	}


	private void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
	}

	private void OnDrawGizmos () {
		if (!Application.isPlaying) return;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(rb.position, right * 5f);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(rb.position, gravVelocity * 5f);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(rb.position, transform.forward * 5f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere((Vector3)rb.position + targetVelocity, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector3)groundCheck.position, groundedRadius);

    }
}
