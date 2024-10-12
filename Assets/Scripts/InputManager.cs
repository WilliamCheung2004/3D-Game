using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.MovementActions movement;

    private PlayerMotor motor;
    private PlayerLook look; 

    //setup componenets before scripts occur
    void Awake()
    {
        playerInput = new PlayerInput();
        movement = playerInput.Movement;
        //reference other script to get methods 
        motor = GetComponent<PlayerMotor>();
        //event listner to jump action - trigger jump function 
        movement.Jump.performed += ctx => motor.Jump();
        look = GetComponent<PlayerLook>();

        movement.Crouch.performed += ctx => motor.Crouch();
        movement.Sprint.performed += ctx => motor.Sprint();
    }

    private void FixedUpdate()
    {
        //tell playermotor to move using value from movment action
        motor.ProcessMove(movement.MoveInput.ReadValue<Vector2>());
    }

    //updates camera movement looking around
    private void LateUpdate()
    {
        look.ProcessLook(movement.Look.ReadValue<Vector2>());
    }

    //called when script activated
    private void OnEnable()
    {
        movement.Enable();
    }

    //called when script deactivated
    private void OnDisable()
    {
        movement.Disable();
    }
}
