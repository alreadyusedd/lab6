using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _JumpForce = 400f;							
	[Range(0, 1)] [SerializeField] private float _CrouchSpeed = .36f;			
	[Range(0, .3f)] [SerializeField] private float _MovementSmoothing = .05f;	
	[SerializeField] private bool _AirControl = false;							
	[SerializeField] private LayerMask _WhatIsGround;							
	[SerializeField] private Transform _GroundCheck;						
	[SerializeField] private Transform _CeilingCheck;							
	[SerializeField] private Collider2D _CrouchDisableCollider;				

	const float _GroundedRadius = .2f;
	private bool _Grounded;            
	const float _CeilingRadius = .3f; 
	private Rigidbody2D rb;
	private bool _FacingRight = true;  
	private Vector3 _Velocity = Vector3.zero;
	private bool _wasCrouching = false;


    [Header("Player Animation Settings")]
    public Animator animator;


    private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

	}

	private void FixedUpdate()
	{
		bool wasGrounded = _Grounded;
		_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(_GroundCheck.position, _GroundedRadius, _WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_Grounded = true;
				
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{

		if (crouch)
		{
			if (Physics2D.OverlapCircle(_CeilingCheck.position, _CeilingRadius, _WhatIsGround))
			{
				crouch = true;
			}
		}

		if (_Grounded || _AirControl)
		{
			if (crouch)
			{
        animator.SetBool("Crouching", true);

        if (!_wasCrouching)
				{
					_wasCrouching = true;
					
				}

				move *= _CrouchSpeed;

				if (_CrouchDisableCollider != null)
					_CrouchDisableCollider.enabled = false;
			} else
			{	
				animator.SetBool("Crouching", false);

				if (_CrouchDisableCollider != null)
					_CrouchDisableCollider.enabled = true;

				if (_wasCrouching)
				{
					_wasCrouching = false;
					
				}
			}

			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);

			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _Velocity, _MovementSmoothing);

			animator.SetFloat("HorizontalMove", Mathf.Abs(move));

			if (_Grounded ==false)
			{
				animator.SetBool("Jumping", true);
			}
			else
			{
				animator.SetBool("Jumping", false);
			}

			if (move > 0 && !_FacingRight)
			{
				Flip();
			}

			else if (move < 0 && _FacingRight)
			{
				Flip();
			}
		}

		if (_Grounded && jump)
		{
			_Grounded = false;
			rb.AddForce(new Vector2(0f, _JumpForce));
		}
	}


	private void Flip()
	{
		_FacingRight = !_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
