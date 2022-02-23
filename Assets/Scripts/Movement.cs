using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 25f;
    [SerializeField] float jumpForce = 25f;
    [SerializeField] LayerMask groundLayer;
    int maxJumps = 2;
    int remainingJumps = 2;

    Rigidbody2D rb;
    EdgeCollider2D edgeCollider;

    bool resetJumps = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        remainingJumps = maxJumps;
    }

    void Update()
    {
        processMovement();
        processJump();
    }

    void processMovement() {
        float horizontalMovement = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontalMovement * movementSpeed, rb.velocity.y);

        processOrientation(horizontalMovement);
    }

    void processOrientation(float horizontalMovement) {
        if(
            horizontalMovement < 0 && isLookingToTheRight() || 
            horizontalMovement > 0 && !isLookingToTheRight()
        ) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool isLookingToTheRight() {
        return transform.localScale.x > 0;
    }

    void processJump() {
        if(isGrounded() && resetJumps) {
            remainingJumps = maxJumps;
        }

        if(!isGrounded()) resetJumps = true;

        if(Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0) {
            remainingJumps--;
            resetJumps = false;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            edgeCollider.bounds.center, 
            new Vector2(edgeCollider.bounds.size.x, edgeCollider.bounds.size.y),
            0f, 
            Vector2.down, 
            0.02f,
            groundLayer
        );

        return raycastHit.collider != null;
    }
}
