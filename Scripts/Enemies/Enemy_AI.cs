
using System.Collections;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;

    private float idleTimer = 0f;
    private float currentIdleDuration = 0f;

    private bool canAttack = true;
    private enum State
    {
        Roaming,
        Stopped,
        Attacking
    }
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Vector2 lastDirection;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }
    private void Update()
    {
        MovementStateControl();
    }
    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Stopped:
                Stop();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosition);
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
            return;
        }
        if (timeRoaming > roamChangeDirFloat)
        {
            if (Random.value < 0.3f) 
            {
                state = State.Stopped;
                currentIdleDuration = Random.Range(minIdleTime, maxIdleTime);
                idleTimer = 0f;
            }
            else
            {
                roamPosition = GetRoamingPosition();
            }
        }
    }


    private void Attacking()
    {
        Transform player = PlayerController.Instance.transform;

        // Exit attack state if player is out of range
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            state = State.Roaming;
            return;
        }

        //  Always update direction to player
        if (stopMovingWhileAttacking)
        {
            enemyPathFinding.StopMoving();
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;
            enemyPathFinding.MoveTo(direction);
        }

        //  Only attack once per cooldown
        if (attackRange != 0 && canAttack)
        {
            canAttack = false;

            if (enemyType != null)
            {
                (enemyType as IEnemy)?.Attack();
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }



    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void Stop()
    {
        enemyPathFinding.StopMoving();
        idleTimer += Time.deltaTime;

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
            return;
        }

        if (idleTimer >= currentIdleDuration)
        {
            roamPosition = GetRoamingPosition();
            state = State.Roaming;
        }
    }

}