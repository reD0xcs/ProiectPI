using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;
    private bool isPlayingSound = false;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    
    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource runSoundEffect;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
        
        if (IsGrounded() && !wasGrounded && dirX != 0 && !isPlayingSound)
        {
            StartCoroutine(PlayRunSound());
        }

        wasGrounded = IsGrounded();
    }


    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            if (!isPlayingSound)
            {
                StartCoroutine(PlayRunSound());
            }

            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            if (!isPlayingSound)
            {
                StartCoroutine(PlayRunSound());
            }

            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
        
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        float raycastDistance = 0.1f; 
        float boxCastDistance = 0.2f;  

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, jumpableGround);
        bool isGroundedRaycast = hit.collider != null;

        bool isGroundedBoxCast = Physics2D.BoxCast(
            coll.bounds.center, 
            coll.bounds.size, 
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
                isGrounded = grounded;
                return;
            }
        }

        isGrounded = false;
    }
    IEnumerator PlayRunSound()
    {
        if (IsGrounded())
        {
            isPlayingSound = true;
            runSoundEffect.Play();
            yield return new WaitForSeconds(runSoundEffect.clip.length);
            isPlayingSound = false;
        }
    }

}
