using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float playerSpeed;

    [SerializeField] private float playerGravity;
    [SerializeField] private float playerJumpPower;

    private CharacterController controller;
    private Camera mainCamera;
    

    private float ySpeed;
    private bool onGround;
    private bool active;

    private int score;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        active = true;

    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {

        Vector3 moveDirection = GetMovementDirection();
        
        Vector3 velocity = moveDirection * playerSpeed;
        
        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Apply any vertical motion, like gravity
        if(!onGround)
            ySpeed -= playerGravity * Time.deltaTime;
        
        velocity.y = ySpeed;
        
        
        controller.Move(velocity * Time.deltaTime);

        if (onGround && !controller.isGrounded)
            Fall();
        
        if(!onGround && controller.isGrounded)
            Land();
        
        
        // Fail logic
        if (transform.position.y < 0 && active)
            Fail();

    }

    private Vector3 GetMovementDirection()
    {
        if (!active) return Vector3.zero;
        
        // Get players input direction
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
        
        // Rotate based on camera direction
        Vector3 moveDirection = mainCamera.transform.TransformDirection(inputDirection);
        moveDirection.y = 0;

        return moveDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Pickup pickupScript))
        {
            CollectPickup(pickupScript);
        }
    }

    private void CollectPickup(Pickup pickupScript)
    {
        // Function returns score to add to player
        int newScore = pickupScript.GetPickedUp();

        // Ignore if already collected
        if (newScore == -1)
            return;
        
        score += newScore;
        
        GameManager.Instance.OnUpdateScore?.Invoke(score);

        //print(score);
    }

    private void Fall()
    {
        onGround = false;
        
        //print("fall");
    }

    private void Jump()
    {
        // Prevent jumping midair
        if (!onGround) return;

        onGround = false;
        
        ySpeed = playerJumpPower;
        
        //print("jump");
    }

    private void Land()
    {
        onGround = true;

        // Setting to a small negative values prevent issues with floating midair and other character controller quirks
        ySpeed = -0.5f;
        
        //print("land");
    }

    private void Fail()
    {
        active = false;
        
        SceneTransitioner.Instance.ReloadCurrentScene("You Died");
        
        //print("dead");
    }
}
