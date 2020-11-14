using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        if (isGrounded)
        {
            if (!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (!isJumping)
                {
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.IDLE);
                }
            }

            //Debug.Log(isCrouching);
            if ((joystick.Vertical < -joystickVerticalSensitivity) && (!isCrouching))
            {
                m_animator.SetInteger("AnimState", (int)PlayerMovementType.CROUCH);
                isCrouching = true;
            }
            else if(joystick.Vertical > -joystickVerticalSensitivity)
            {
                isCrouching = false;
            }
            
            if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
            {
                // jump
                Jump();
            }
            else
            {
                isJumping = false;
            }
        }

    }

    public void Jump()
    {
        if (!isJumping)
        {
            m_rigidBody2D.AddForce(Vector2.up * verticalForce);
            m_animator.SetInteger("AnimState", (int)PlayerMovementType.JUMP);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            transform.position = spawnPoint.position;
        }
    }
}
