using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float walkingSpeed = 12f;
    public float sprintingSpeed = 24f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float speed;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        speed = 12f;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //speed = Input.GetKey(KeyCode.LeftShift) ? sprintingSpeed : walkingSpeed;

        if(Input.GetKey(KeyCode.LeftShift) && speed < 100f)
        {
           speed += 0.5f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? sprintingSpeed : walkingSpeed;
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
