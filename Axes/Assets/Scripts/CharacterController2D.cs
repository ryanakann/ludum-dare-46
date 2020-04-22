using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : Entity
{
    [Header("Movement")]
	[SerializeField] public float jumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] public float speed = 2f;
    Timer jump_cooldown = new Timer(0.5f);

    SpriteRenderer sr;

	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask groundLayer;							// A mask determining what is ground to the character

    [Header("References")]
	[SerializeField] private Transform groundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D crouchDisableCollider;				// A collider that will be disabled when crouching

	const float groundedRadius = .3f; // Radius of the overlap circle to determine if grounded
	const float ceilingRadius = .3f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rb;
	private bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	private Vector3 gravVelocity;
	private Vector2 gravity;
	private Vector2 gravityLF;
	private Vector3 right;
	private Vector3 targetVelocity;

	private Animator anim;

    [HideInInspector] public bool locked = false;
    private bool lockedLF;
    [HideInInspector] public bool frozen = false;
    private bool frozenLF;

    [HideInInspector] public int jumpCount;

    [Header("SFX")]
    public float footstepFrequencyModifier = 1f;
    private float footstepAnimationTime;
    private float footstepNextThreshold;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool groundedLF = false;

    float move; bool crouch; bool jump; bool jumpStay;


    protected override void Awake() {
		base.Awake();
        sr = GetComponent<SpriteRenderer>();
        jump_cooldown.Reset();
		rb = GetComponent<Rigidbody2D>();
		gravity = Physics2D.gravity;
		gravityLF = gravity;
		if (OnLandEvent == null)OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null) OnCrouchEvent = new BoolEvent();

		jumpCount = 0;

		anim = GetComponent<Animator>();

        footstepAnimationTime = 0f;
        footstepNextThreshold = footstepFrequencyModifier;
	}

	protected override void FixedUpdate() {
		bool wasGrounded = grounded;
		grounded = false;

        groundCheck.position = new Vector2(groundCheck.position.x, sr.bounds.center.y - sr.bounds.extents.y + groundedRadius);

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

        HandleMovement();

        frozenLF = frozen;
        lockedLF = locked;
	}


	public void Move(float move, bool crouch, bool jump, bool jumpStay) {
        if (locked) return;
        this.move = move;
        this.crouch = crouch;
        this.jump = jump;
        this.jumpStay = jumpStay;
	}

    void HandleMovement()
    {
        anim.SetFloat("x", move);
        velocity = rb.velocity;

        //CHECK CROUCH
        /******************************************************************/
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            Collider2D[] colls = Physics2D.OverlapCircleAll(ceilingCheck.position, ceilingRadius, groundLayer);
            foreach (Collider2D c in colls)
            {
                if (!c.isTrigger)
                {
                    crouch = true;
                    break;
                }
            }
        }

        //CHECK GRAVITY
        /******************************************************************/
        gravity = Physics2D.gravity * rb.gravityScale;
        if ((gravityLF - gravity).sqrMagnitude > 0.001f)
        {
            //Always face away from gravity
            rb.freezeRotation = false;
            transform.forward = Vector3.forward;
            transform.up = -gravity;
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = true;
        }
        gravityLF = gravity;

        //JUMP MODIFIER
        /******************************************************************/
        // If not holding down jump and going up
        if (!grounded && !jumpStay && Vector2.Dot(gravity, velocity) < 0f)
        {
            //print("1");
            rb.gravityScale = 6f;
        }
        else if (!grounded && Vector2.Dot(gravity, velocity) > 0f)
        {
            //print("2");
            rb.gravityScale = 6f;
        }
        else
        {
            //print("3");
            rb.gravityScale = 3f;
        }

        //PLAYER CONTROL
        /******************************************************************/
        //Only if grounded or airControl is turned on
        if (grounded || m_AirControl)
        {
            // If crouching
            if (crouch)
            {
                if (!groundedLF)
                {
                    groundedLF = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = true;

                if (groundedLF)
                {
                    groundedLF = false;
                    OnCrouchEvent.Invoke(false);
                }
            }


            // Move the character by finding the target velocity
            gravVelocity = Vector3.Project(velocity, gravity);

            right = transform.right;
            targetVelocity = gravVelocity + right * move * speed;
            //print("TargetVel: " + targetVelocity + "\tMove: " + move);
            //Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref velocity, m_MovementSmoothing);

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
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }


        //JUMP CONTROL
        /******************************************************************/
        if (grounded && jump && jump_cooldown.Check())
        {
            grounded = false;

            velocity = right * move * speed; // Zero out jump velocity
            velocity -= (Vector3)gravity.normalized * jumpForce;

            jumpCount++;

            anim.SetTrigger("jump");
        }

        rb.velocity = velocity;
    }

	public void Die () {
		anim.SetTrigger("hurt");
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
