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

    [Header("Climbing")]
    public LayerMask climbableMask;
    public Transform climbDetector;       // điểm raycast ngang ngực player
    public float climbCheckDistance = 0.6f;
    public float climbSpeed = 3f;
    public float vaultCheckHeight = 1.5f;  // raycast kiểm tra đỉnh tường
    public float vaultForwardOffset = 0.6f;
    public float vaultDuration = 0.3f;
    private bool isVaulting = false;
    private RaycastHit wallHit;

    [Header("trang thai hien tai")]
    public bool isAttacking = false;
    public bool isRunning = false;
    public bool isClimbing = false;
    public bool isJumping = false;

    public Animator animator;
    private Rigidbody rb;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        CheckGround();

        if (isVaulting) return; // đang vault thì khóa toàn bộ input khác

        CheckClimbable();

        if (isClimbing)
        {
            PlayerClimbing();
        }
        else
        {
            PlayerRunning();
        }
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
        if (isClimbing) return; // không cho nhảy khi đang leo

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isJumping", true);
    }

    void CheckGround()
    {
        if (isClimbing) return; // đang leo thì bỏ qua check ground như cũ

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (!isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else if (!isGrounded && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
    }

    public void PlayerAttacking()
    {
        isAttacking = true;
        // isAttacking = false;
    }

    void CheckClimbable()
    {
        Vector3 origin = climbDetector.position;
        bool hitWall = Physics.Raycast(origin, transform.forward, out wallHit, climbCheckDistance, climbableMask);

        if (hitWall && joystick.Vertical > 0.1f && !isClimbing)
        {
            StartClimbing();
        }
        else if (!hitWall && isClimbing)
        {
            StopClimbing();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        animator.SetBool("isClimbing", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        isRunning = false;

        Vector3 lookDir = -wallHit.normal;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void PlayerClimbing()
    {
        bool stillOnWall = Physics.Raycast(climbDetector.position, transform.forward, climbCheckDistance, climbableMask);
        if (!stillOnWall)
        {
            StopClimbing();
            return;
        }

        bool topBlocked = Physics.Raycast(climbDetector.position + Vector3.up * vaultCheckHeight, transform.forward, climbCheckDistance, climbableMask);
        if (!topBlocked && joystick.Vertical > 0.1f)
        {
            StartCoroutine(VaultOverWall());
            return;
        }

        float verticalInput = joystick.Vertical;
        rb.linearVelocity = new Vector3(0f, verticalInput * climbSpeed, 0f);

        animator.SetFloat("climbSpeed", verticalInput);
    }

    IEnumerator VaultOverWall()
    {
        isVaulting = true;
        isClimbing = false;
        animator.SetBool("isClimbing", false);
        animator.SetBool("isJumping", true); 

        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * vaultCheckHeight + transform.forward * vaultForwardOffset;

        float elapsed = 0f;
        while (elapsed < vaultDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / vaultDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        rb.useGravity = true;
        animator.SetBool("isJumping", false);
        isVaulting = false;
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
        animator.SetBool("isClimbing", false);
        animator.SetFloat("climbSpeed", 0f);
    }
}