using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player's movement, such as running, walking, sneaking 
public class PlayerMovement : MonoBehaviour
{   
    // Speed multiplier of the movement
    [SerializeField] float moveSpeed = 5f;


    Rigidbody2D rb;
    Animator animator;
    Vector2 movement;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize, so the player is not faster by moving diagonally
        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogError("No Rigidbody2D found in PlayerMovement.cs");
        }
    }


}
