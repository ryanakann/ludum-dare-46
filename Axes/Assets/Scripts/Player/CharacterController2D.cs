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
    private Vector3 gravVelocity;
    private Vector2 gravity;
    private Vector2 gravityLF;
    private Vector3 right;
    private Vector3 targetVelocity;
    private float move; private bool crouch; private bool jumpDown; private bool jumpStay;
    private bool groundedLF = false;
    private bool facingRight;
    
    #endregion

    protected override void Awake() {
		base.Awake();
        
        //Component References
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

        //Footsteps
        footstepAnimationTime = 0f;
        footstepNextThreshold = footstepFrequencyModifier;
	}

	protected override void FixedUpdate() {
		bool wasGrounded = grounded;
		grounded = false;

        // groundCheck.position = new Vector2(groundCheck.position.x, sr.bounds.center.y - sr.bounds.extents.y + groundedRadius);

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
        gravity = Physics2D.gravity * rb.gravityScale;

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
            rb.gravityScale = lowJumpMultiplier;
        } 
        // Falling
        else if (!grounded && Vector2.Dot(gravity, velocity) > 0f) {
            rb.gravityScale = fallMultiplier;
        } 
        // Grounded
        else {
            rb.gravityScale = 1f;
        }
        rb.gravityScale *= gravityMultiplier;


        //PLAYER CONTROL
        /******************************************************************/
        //Only allow movement if grounded, or airborne when airControl is turned on
        if (grounded || airControl) {
            // Calculate velocity caused by gravity
            gravVelocity = Vector3.Project(velocity, gravity);

            right = transform.right;
            targetVelocity = gravVelocity + right * move * baseMoveSpeed; //Velocity of player = velocity due to gravity plus orthogonal player input movement

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

            velocity = right * move * baseMoveSpeed; // Zero out jump velocity
            float jumpVelocity = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * gravityMultiplier * maxJumpHeight);

            velocity -= (Vector3)gravity.normalized * jumpVelocity;

            anim.SetTrigger("jump");
        }

        //Finally, set velocity
        rb.velocity = velocity;
    }

	public void Die () {
		anim.SetTrigger("hurt");
	}


	private void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
