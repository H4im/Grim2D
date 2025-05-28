using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController> {

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dashspeed = 2f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool isDashing = false;

    private const string horizontal = "moveX";
    private const string verticle = "moveY";

    protected override void Awake()
    {
        base.Awake();

        Debug.Log("PlayerController Instance assigned.");
        playerControls =new PlayerControls();
        rb=GetComponent<Rigidbody2D>();
        myAnimator=GetComponent<Animator>();
        mySpriteRenderer=GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquipStartingWeapon();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }
    private void Update()
    {
        PlayerInput();

    }
    private void FixedUpdate()
    {
        Move();
    }
    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
    private void PlayerInput()
    {
        if(PlayerHealth.Instance.isDead)
            return;


        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);

        if (movement.x != 0 || movement.y != 0)
        {
            lastMovementDirection = movement;

        }
        else
        {

            if (lastMovementDirection.x < 0)
            {
                myAnimator.Play("Idle_Left");
            }
            else if (lastMovementDirection.x > 0)
            {
                myAnimator.Play("Idle_Right");
            }
            else if (lastMovementDirection.y > 0)
            {
                myAnimator.Play("Idle_Up");
            }
            else if (lastMovementDirection.y < 0)
            {
                myAnimator.Play("Idle_Down");
            }
        }
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashspeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }
    private IEnumerator EndDashRoutine()
    {
        float dashTime = .1f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting=false;
        yield return new WaitForSeconds(dashCD);
        isDashing=false;
    }
    }
