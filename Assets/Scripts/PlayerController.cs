using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour 
{
    [SerializeField]
	[Range(3.0f,50.0f)]
	private float MovementSpeed = 3.0f;
    [SerializeField]
	private float JumpForce = 500.0f; 

    //MultiJump variables
    [SerializeField]
    private int MultiJump = 2;
    private int _multiJump;

    [SerializeField]
	private Transform GroundCheck; //Ground check Collider
    [SerializeField]
	private LayerMask GroundLayerMask;

	private float _movementHorizontal;
	private float _movementVertical;

    [SerializeField]
    [Range(0.0f,10.0f)]
    private float _slideTime = 1.0f; //Amount of time in slide animation
    private float _attackTime = 1.0f; //Amount of time in melee animation 
    private float _shootTime = 1.0f; //Amount of time in shoot animation

	private Transform _transform;
	private Rigidbody2D _rigidbody2d;
	private Animator _animator;
    private AnimationClip clip;

	private bool IsFacingRight = true;
	private bool _isRunning = false;
	private bool _isGrounded = true;
    private bool _isSlide = false;
    private bool _isAttack = false;
    private bool _isShooting = false;


    private int _playerLayer;
	private int _platformLayer;

	void Awake()
	{

		_transform = transform; //transform optimization for quick execution
        _multiJump = MultiJump; //store amount of multi-jumps wanted
		_rigidbody2d = GetComponent<Rigidbody2D> ();
		_animator = GetComponent<Animator> ();
        if (_animator == null)
            Debug.Log("Error: No animator");
        else
            Debug.Log("Animator loaded");

        _playerLayer = gameObject.layer; //player layer mask
		_platformLayer = LayerMask.NameToLayer ("Platform"); //platform layer mask
	
	}
    void Start()
    {
        UpdateAnimClipTimes();
    }

    void Update()
    {
        _movementHorizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        if (_movementHorizontal != 0)
        {
            _isRunning = true;
            if (_isGrounded && CrossPlatformInputManager.GetButton("Fire3"))
            {
                _isSlide = true;
                Invoke("Slide", _slideTime); //slide animation for _slideTime amount of time
            }
        }

        else
        {
            _isRunning = false;
            _isSlide = false;
        }
        
        if ((!_isSlide && CrossPlatformInputManager.GetButton("Fire2")) && (_isGrounded && !_isRunning)) //melee
        {
            _isAttack = true;
            Invoke("Attack", _attackTime); //melee animation for _attackTime amount of time
        }

        

        if ((!_isSlide && CrossPlatformInputManager.GetButton("Fire1")) && (_isGrounded && !_isRunning))
        {
            _isShooting = true;
            Invoke("Shoot", _shootTime);//shoot animation for _shootTime amount of time
        }
        GameManager.gameManager._canShoot = _isShooting;

        _animator.SetBool("IsRunning", _isRunning);
        _animator.SetBool("SlideKey", _isSlide);
        if(IsInvoking("Attack"))
        _animator.SetBool("IsAttack", _isAttack);
        if(IsInvoking("Shoot"))
        _animator.SetBool("IsShooting", _isShooting);


        _movementVertical = _rigidbody2d.velocity.y;

		if (IsInvoking ("Slide") && (_isSlide && !_isGrounded)) 
		{
			CancelInvoke ("Slide"); //if ground not true, cancel slide animation
			_isSlide = false;
		}

		_isGrounded = Physics2D.Linecast (_transform.position, GroundCheck.position, GroundLayerMask);

		if (_isGrounded && CrossPlatformInputManager.GetButtonDown("Jump")) //jump if it is on the ground
		{

            _multiJump = MultiJump;
			_movementVertical = 0.0f;
			_rigidbody2d.AddForce (Vector2.up * JumpForce);
		}

		if (CrossPlatformInputManager.GetButtonUp ("Jump") && (_movementVertical >= 0.0f))//if it is mid-air and wants to jump more
			if(_multiJump>0) //if more jump avalavable
            {
                _movementVertical = 0.0f;
                _rigidbody2d.AddForce(Vector2.up * JumpForce);//jump
                _multiJump--;//decrease counter
                _animator.Play("jump");//play animation
            }
            else
            {
                _multiJump = MultiJump; //reset to start value
                _movementVertical = 0.0f; //reset vertical speed
            }
                
        _animator.SetBool ("IsGrounded", _isGrounded); //play idle animation if only grounded

		
		Vector2 movement = new Vector2 (_movementHorizontal * MovementSpeed, _movementVertical);
		_rigidbody2d.velocity = movement;

		Physics2D.IgnoreLayerCollision (_playerLayer, _platformLayer, (_movementVertical > 0.0f));
	}

	void LateUpdate()
	{
		Flip ();
        
    }

    private void Slide()
    {
        _isSlide = false;
    }

    private void Attack()
    {
        _isAttack = false;
    }

    private void Shoot()
    {
        _isShooting = false;
    }

	private void Flip() //flip facing direction
	{
		Vector3 localScale = _transform.localScale;
		if (_movementHorizontal > 0.0f)
			IsFacingRight = true;
		else if (_movementHorizontal < 0.0f)
			IsFacingRight = false;

		if ((IsFacingRight && (localScale.x < 0.0f)) || (!IsFacingRight && (localScale.x > 0.0f)))
			localScale.x *= -1.0f;

		_transform.localScale = localScale;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == _platformLayer) 
		{
			_transform.SetParent (other.transform);
            _isGrounded = true;
            _animator.SetBool("IsGrounded", _isGrounded);
        }
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.layer == _platformLayer) 
		{
			_transform.parent = null;
		}
	}

	void OnDrawGizmos() //draw line between player pivot and ground check object
	{
		Gizmos.DrawLine (transform.position, GroundCheck.position);
	}

    void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Melee":
                    _attackTime = clip.length;
                    break;
                case "Shoot":
                    _shootTime = clip.length;
                    GameManager.gameManager.ShootAnimTime = _shootTime;
                    break;
            }
        }
    }
}
