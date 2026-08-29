using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Player : MonoBehaviour
{
    [Header("Controller")]
    public FixedJoystick joystick;

    [Header("Di chuyen")]
    private float speed = 6f;
    private float jumpForce = 5f;

    [Header("Kiem tra cham dat")]
    public float playerHeight;
    public LayerMask groundMask;

    [Header("Camera")]
    public Transform cameraPlayer;

    [Header("trang thai hien tai")]
    public bool isAttacking = false;
    public bool isRunning = false;
    public bool isClimbing = false;
    public bool isJumping = false;

    public Animator animator;
    private Rigidbody rb;
    public bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();

        PlayerRunning();
    }


    void PlayerRunning()
    {
        Vector3 camForward = cameraPlayer.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraPlayer.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * joystick.Vertical + camRight * joystick.Horizontal;

        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            isRunning = true;
            animator.SetBool("isRunning", true);

            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatVelocity.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(flatVelocity);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            isRunning = false;
        }
    }

    public void PlayerJumping()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isJumping",true);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f , groundMask);

        if (!isGrounded)
        {
            animator.SetBool("isJumping",true);
        }
        else if (!isGrounded && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping",false);
        }
    }

    void PlayerClimbing()
    {
        
    }

    public void PlayerAttacking()
    {
        isAttacking = true;



        // isAttacking = false;
    }


}
