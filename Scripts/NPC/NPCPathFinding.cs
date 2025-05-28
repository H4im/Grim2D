using UnityEngine;

public class NPCPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    public Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Vector2 LastMoveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // Move the NPC
        if (knockback.GettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        // Update animation direction
        UpdateAnimationDirection();
    }

    public void UpdateLastDirection(Vector2 direction)
    {
        LastMoveDir = direction;
        SetIdleDirection(LastMoveDir); // Ensure it plays the idle animation for the last direction
    }

    private void UpdateAnimationDirection()
    {
        // If the NPC is moving
        if (moveDir.x != 0 || moveDir.y != 0)
        {
            SetAnimationDirection(moveDir); // Set walking animation if moving
        }
        else
        {
            // If the NPC is stopped, ensure idle animation triggers correctly
            SetIdleDirection(LastMoveDir); // Set idle animation based on the last direction
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        if (targetPosition != Vector2.zero)
        {
            moveDir = targetPosition;
            // Debugging moveDir
        }
        else
        {
            moveDir = Vector2.zero; // Ensure NPC is not moving
            // Debug.Log("NPC has stopped");
        }
    }

    private void SetIdleDirection(Vector2 direction)
    {
        // Ensure we reset all idle animation triggers before setting the new one
        myAnimator.ResetTrigger("IdleRightTrigger");
        myAnimator.ResetTrigger("IdleLeftTrigger");
        myAnimator.ResetTrigger("IdleUpTrigger");
        myAnimator.ResetTrigger("IdleDownTrigger");

        // Set the correct trigger based on the last move direction

        if (Mathf.Abs(LastMoveDir.x) > Mathf.Abs(LastMoveDir.y))
        {
            if (LastMoveDir.x > 0) // Idle right
            {
                myAnimator.SetTrigger("IdleRightTrigger");
            }
            else if (LastMoveDir.x < 0) // Idle left
            {
                myAnimator.SetTrigger("IdleLeftTrigger");
            }
        }
        else { 
        if (LastMoveDir.y > 0) // Idle up
            {
                 myAnimator.SetTrigger("IdleUpTrigger");
            }
        else if (LastMoveDir.y < 0) // Idle down
            {
                myAnimator.SetTrigger("IdleDownTrigger");

            }
        }

}

private void SetAnimationDirection(Vector2 direction)
    {
        // Check if the absolute value of x is greater than the absolute value of y
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement (left or right)
            if (direction.x > 0) // Moving right
            {
                myAnimator.SetFloat("moveX", 1);
                myAnimator.SetFloat("moveY", 0);
                mySpriteRenderer.flipX = false; // Face right
            }
            else if (direction.x < 0) // Moving left
            {
                myAnimator.SetFloat("moveX", -1);
                myAnimator.SetFloat("moveY", 0);
                mySpriteRenderer.flipX = true; // Face left
            }
        }
        else
        {
            // Vertical movement (up or down)
            if (direction.y > 0) // Moving up
            {
                myAnimator.SetFloat("moveX", 0);
                myAnimator.SetFloat("moveY", 1);
            }
            else if (direction.y < 0) // Moving down
            {
                myAnimator.SetFloat("moveX", 0);
                myAnimator.SetFloat("moveY", -1);
            }
        }
    }
    private void ChangeDirection()
    {
        if (Random.value > 0.4f) // 50% chance
        {
            moveDir = -moveDir; // Reverse movement direction
        }
        else
        {
            moveDir = Vector2.zero; // Stop moving (idle)
        }
    }


}

