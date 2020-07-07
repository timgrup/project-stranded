using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform camera;
    [SerializeField] Animator animator;

    //Velocity Variables
    [SerializeField] float speed = 1.0f;
    [SerializeField] float speedSprint = 2.0f;
    [SerializeField] float gravity = -9.81f; //Earth Gravity
    public float jumpHeight = 3.0f;
    Vector3 velocity;

    //Turn Variables
    [SerializeField] float turnSmoothTime = 0.1f;
    float turnSmooth;

    //Ground Check Variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    bool isSprinting = false;


    void Update()
    {
        GroundCheck();

        //Set Velocity to 0 if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        Move();
        Jump();
        ApplyGravity();
    }

    private void Move()
    {
        //Get Input
        float horizontalThrow = Input.GetAxis("Horizontal");
        float verticalThrow = Input.GetAxis("Vertical");

        SprintControl();
        var speed = this.speed;
        if (isSprinting) speed = speedSprint;

        //Direction
        Vector3 rawDirection = new Vector3(horizontalThrow, 0f, verticalThrow).normalized * speed * Time.deltaTime;
        Vector3 direction = rawDirection;

        float targetAngle = transform.eulerAngles.y;
        if (Input.GetKey(KeyCode.Mouse1)) //Right Click
        {
            targetAngle = camera.eulerAngles.y;
        }
        else if (rawDirection.magnitude > Mathf.Epsilon) //Moving because of input
        {
            targetAngle = Mathf.Atan2(rawDirection.x, rawDirection.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            direction = Vector3.forward * speed * Time.deltaTime;
        }
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 cameraDirection = Quaternion.Euler(0f, angle, 0f) * direction;

        characterController.Move(cameraDirection);

        if (cameraDirection.magnitude >= Mathf.Epsilon)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void SprintControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = false;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        if (velocity.y < -0.5f)
        {
            animator.SetFloat("velocity", velocity.y);
        }
        else
        {
            animator.SetFloat("velocity", 0f);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded != wasGrounded && wasGrounded == false)
        {
            onGroundLand();
        }
    }

    void onGroundLand()
    {
        animator.SetTrigger("land");
    }
}
