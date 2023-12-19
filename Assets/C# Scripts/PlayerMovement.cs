using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Private Variables

    private Rigidbody2D _rb;
    private BoxCollider2D _coll;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private float _dirX;
    private bool _isPlayingSound = false;
    private bool _isGrounded = false;
    private bool _wasGrounded = false;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource runSoundEffect;

    #endregion
    
    private enum MovementState { Idle, Running, Jumping, Falling }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        _dirX = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(_dirX * moveSpeed, _rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
        
        if (IsGrounded() && !_wasGrounded && _dirX != 0 && !_isPlayingSound)
        {
            StartCoroutine(PlayRunSound());
        }

        _wasGrounded = IsGrounded();
    }


    private void UpdateAnimationState()
    {
        MovementState state;
        if (_dirX > 0f)
        {
            if (!_isPlayingSound)
            {
                StartCoroutine(PlayRunSound());
            }

            state = MovementState.Running;
            _sprite.flipX = false;
        }
        else if (_dirX < 0f)
        {
            if (!_isPlayingSound)
            {
                StartCoroutine(PlayRunSound());
            }

            state = MovementState.Running;
            _sprite.flipX = true;
        }
        else
        {
            state = MovementState.Idle;
        }

        if (_rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }
        else if (_rb.velocity.y < -0.1f)
        {
            state = MovementState.Falling;
        }
        
        _anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        float raycastDistance = 0.1f; 
        float boxCastDistance = 0.2f;  

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, jumpableGround);
        bool isGroundedRaycast = !object.ReferenceEquals(hit.collider, null);
        
        bool isGroundedBoxCast = Physics2D.BoxCast(
            _coll.bounds.center, 
            _coll.bounds.size, 
            0f, 
            Vector2.down, 
            boxCastDistance, 
            jumpableGround
        );

        return isGroundedRaycast || isGroundedBoxCast;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundedState(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UpdateGroundedState(collision, false);
    }

    private void UpdateGroundedState(Collision2D collision, bool grounded)
    {
        float groundedThreshold = 0.1f;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > groundedThreshold)
            {
                _isGrounded = grounded;
                return;
            }
        }

        _isGrounded = false;
    }
    IEnumerator PlayRunSound()
    {
        if (IsGrounded())
        {
            _isPlayingSound = true;
            runSoundEffect.Play();
            yield return new WaitForSeconds(runSoundEffect.clip.length);
            _isPlayingSound = false;
        }
    }
}