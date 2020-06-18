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
    [SerializeField] float gravity = -9.81f; //Earth Gravity
    public float jumpHeight = 3.0f;

    //Turn Variables
    [SerializeField] float turnSmoothTime = 0.1f;
    float turnSmooth;

    //Ground Check Variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        //Ground Check
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded != wasGrounded && wasGrounded == false)
        {
            onGroundLand();
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Get Input
        float horizontalThrow = Input.GetAxis("Horizontal");
        float verticalThrow = Input.GetAxis("Vertical");

        //Direction Vector
        Vector3 rawDirection = new Vector3(horizontalThrow, 0, verticalThrow).normalized;
        Vector3 direction = rawDirection * speed * Time.deltaTime;

        if (direction.magnitude > Mathf.Epsilon)
        {
            //Rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime); //Smooth Rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Move Player
            animator.SetBool("isWalking", true);
            Vector3 directionCamera = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(directionCamera.normalized * speed * Time.deltaTime); //Normalize Vector when moving
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void onGroundLand()
    {

    }
}
