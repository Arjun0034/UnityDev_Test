using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 5f;
    public float fallTimeLimit = 3f;

    private Rigidbody rb;
    private Animator animator;

    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask ground;

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    public float groundDistance = 0.4f;
    public float downwardForce = 10f;

    private bool isGrounded;
    private bool canMove = true; 
    private float fallTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canMove) return; 

        CheckGroundStatus();
        MovementHandler();
        Jump();
        HandleFalling();
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
    }

    private void MovementHandler()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.deltaTime);

            animator.SetFloat("Running", direction.magnitude);
            animator.SetBool("Idle", false);
            animator.SetBool("Falling", false);
        }
        else
        {
            animator.SetFloat("Running", 0f);
            animator.SetBool("Idle", true);
            animator.SetBool("Falling", false);
        }

        if (isGrounded)
        {
            animator.SetBool("Falling", false);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
            animator.SetBool("Falling", true);
        }
    }

    private void HandleFalling()
    {
        if (!isGrounded)
        {
            animator.SetBool("Falling", true);
            fallTime += Time.deltaTime;
            rb.AddForce(Vector3.down * downwardForce * Time.deltaTime, ForceMode.Acceleration);

            if (fallTime >= fallTimeLimit)
            {
                GetComponent<GameManager>().GameOver();
            }
        }
        else
        {
            fallTime = 0f;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void EnableMovement(bool enable)
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
