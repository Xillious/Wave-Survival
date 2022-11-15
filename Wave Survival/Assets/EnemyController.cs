using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Aggro,
        Chase,
        Wander
    }

    public float test;
    public float aggroRange = 4f;
    public float chaseRange = 2f;
    public float aggroSpeed = 2f;
    public float chaseSpeed = 4f;
    public float moveSpeed;
    public float speedIncrease = .2f;
    public float wanderRadius = 4f;

    bool isFacingRight;

    private Rigidbody2D rb;

    public EnemyState enemyState;

    public Transform targetPlayer;

    public Vector3 wanderPosition;

    private void Awake()
    {
        //targetPlayer = FindObjectOfType<PlayerController>().transform;
    }

    IEnumerator Start()
    {

        rb = GetComponent<Rigidbody2D>();

        while (true)
        {
            switch (enemyState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;

                case EnemyState.Aggro:
                    Aggro();
                    break;

                case EnemyState.Chase:
                    Chase();
                    break;

                case EnemyState.Wander:
                    Wander();
                    break;

            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRangeCheck(aggroRange))
        {
            targetPlayer = GameObject.FindWithTag("Player").transform;
        }
        else
        {
            targetPlayer = null;
        }

        DebugControls();

    }

    void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    void Idle()
    {
        if (PlayerRangeCheck(aggroRange))
        {
            ChangeState(EnemyState.Aggro);
        }
    }

    void Wander()
    {
        wanderPosition = RandomPos(transform.position, wanderRadius);

        /*
        if (transform.position != wanderPosition)
        {
            wanderPosition = new Vector3(RandomPosition(7f, -7f), RandomPosition(7f, -7f), 0);
        }
        */

        Movement(RandomPos(transform.position, 3f));
    }

    IEnumerator NewWander()
    {

        Movement(RandomPos(transform.position, 2f));
        yield return new WaitForSecondsRealtime(0f);
    }

    void Aggro()
    {
        //range where enemy starts moving towards the player - slower speed

        //moveSpeed = 2f;
        moveSpeed = Mathf.Lerp(chaseSpeed, aggroSpeed, speedIncrease * Time.deltaTime);

        if (targetPlayer != null)
        {
            Movement(targetPlayer.transform.position);
        }

        if (!PlayerRangeCheck(aggroRange))
        {
            ChangeState(EnemyState.Idle);
        }

        if (PlayerRangeCheck(chaseRange))
        {
            ChangeState(EnemyState.Chase);
        }
    }

    void Chase()
    {
        //closer range where the enemy moves faster towards the player - faster speed

        //moveSpeed = 4f;
        moveSpeed = Mathf.Lerp(aggroSpeed, chaseSpeed, speedIncrease * Time.deltaTime);

        if (targetPlayer != null)
        {
            Movement(targetPlayer.transform.position);
        }

        if (!PlayerRangeCheck(chaseRange))
        {
            ChangeState(EnemyState.Aggro);
        }
    }

    void Movement(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }


    private bool PlayerRangeCheck(float distance)
    {
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) < distance)
        {
            return true;
        }
        else
            return false;
    }

    private float RandomPosition(float min, float max)
    {
        float randomPosition;

        randomPosition = Random.Range(min, max);

        return randomPosition;
    }

    private Vector3 RandomPos(Vector3 center, float raduis)
    {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * raduis;
    }

    void DebugControls()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(enemyState);
            Debug.Log(targetPlayer);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeState(EnemyState.Wander);
            //Debug.Log(RandomPos(transform.position, 3f));
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        //Gizmos.DrawWireSphere(transform.position, wanderRadius);


    }


}
