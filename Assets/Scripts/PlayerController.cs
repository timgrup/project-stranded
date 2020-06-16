using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    [SerializeField] CharacterController characterController;

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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime); //Smooth Rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Move Player
            characterController.Move(direction);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
