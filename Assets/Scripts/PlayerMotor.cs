using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    public float speed = 5f;
    public float defaultSpeed = 5f;
    //negative to simulate fall
    public float gravity = -9.8f;
    public float defaultjumpHeight = 1f;
    public float jumpHeight = 1f;
    private bool lerpCrouch;
    public float crouchTimer;
    private bool crouching;
    private bool sprinting;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //using isGrounded property set boolean value based on that
        IsGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            
                controller.height = Mathf.Lerp(controller.height, 1, p);
                else
                    controller.height = Mathf.Lerp(controller.height, 2, p);
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    //recieve input and apply to controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //map y to z to hand forward and backward movement
        moveDirection.z = input.y;
        //apply movement in the direction with current speed 
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        //makes sure player is falling when not on ground
        playerVelocity.y += gravity * Time.deltaTime;

        if(IsGrounded && playerVelocity.y < 0)
            //reset fall speed ensure player is on ground
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }

    //check if player is on the ground, if so can jump
    public void Jump()
    {
        if (IsGrounded)
        {
            //find initial velocity - u = sqrt(2*a*s) but here simulating a game so use -3.0f 
            //to simulate gravity
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
        jumpHeight = defaultjumpHeight;

        //lower speed whenever crouching and jump crouching will have lower jump 
        if (crouching && sprinting)
        {
            speed = 2;
            jumpHeight /= 2;
        }
        else if (crouching)
        {
            speed = 1;
            jumpHeight /= 2;
        }
        else
        {
            speed = defaultSpeed;
            jumpHeight /= 2;
        }
        
    }

    public void Sprint()
    {
        jumpHeight = defaultjumpHeight;

        sprinting = !sprinting;

        //increase speed slightly if crouching 
        if (sprinting && crouching)
        {
            speed = 2;
            jumpHeight /= 2;
        }

        //more speed if only sprinting 
        else if (sprinting)
        {
            speed = 15;
        }

        else
        {
            speed = defaultSpeed;
        }

    }
}
