using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemyController : MonoBehaviour
{

    public enum EnemyState
    {
        Idle,
        Aggro,
        Wander
    }

    public string enemyName;


    public float moveSpeed = 2f;
    public float testPosition;
    public float bigWanderMin, bigWanderMax;
    public float smallWanderMin, smallWanderMax;
    public float aggroRange;

    private bool isFacingRight;
    private bool isWandering = false;
    private bool hasReachedWanderPosition = false;

    public EnemyState enemyState;

    private Rigidbody2D rb;

    private Vector2 targetPosition;

    private Vector3 currentPosition;
    private Vector3 newPosition;
    private Vector3 previousPosition;

    private Vector3 targetPos;
    private Vector3 oldPos;
    private Vector3 currentPos;

    private Vector3 target;

    public Transform targetPlayer;

    private void Awake()
    {
        targetPlayer = FindObjectOfType<PlayerController>().transform;
        //targetPosition = new Vector2(RandomPosition(bigWanderMin, bigWanderMax), RandomPosition(bigWanderMin, bigWanderMax));

    }

    IEnumerator Start()
    {

        target = new Vector3(3, 3, 0);

        rb = GetComponent<Rigidbody2D>();

        //enemyState = EnemyState.Idle;


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

                case EnemyState.Wander:
                    Wander();
                    break;

            }
            yield return new WaitForEndOfFrame();
        }

    }


    void Update()
    {
        DebugControls();

        PlayerPosition();

        //currentPosition = gameObject.transform.position;

    }

    void Movement(Vector2 targetPos)
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, moveSpeed * Time.deltaTime);

        //Vector2.MoveTowards(targetPosition);

        //transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);


        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (currentPosition != newPosition)
        {

        }

        /*
        if (Vector2.Distance(transform.position, newPosition) < 0.1f || (hasReachedWanderPosition = false))
        {
            //makes it get stuck on the point until it can wiggle off
            ChangeState(EnemyState.Idle);
            hasReachedWanderPosition = true;
        }

        */
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

    void PlayerPosition()
    {
        if ((transform.position.x < targetPlayer.transform.position.x) && isFacingRight)
        {
            Flip();
        }
        else if ((transform.position.x > targetPlayer.transform.position.x) && !isFacingRight)
        {
            Flip();
        }
    }


    private float RandomPosition(float min, float max)
    {
        float randomPosition;


        transform.position = new Vector3(transform.position.x + min, transform.position.y + max);
        //randomPosition = Random.Range(min, max);
        //randomPosition = Random.Range(min, max);
        randomPosition = Random.Range(min, max);

        return randomPosition;
    }

    private int RandomPositionWhole(int min, int max)
    {
        int pos;

        pos = Random.Range(min, max);

        return pos;
    }

    void Idle()
    {

        hasReachedWanderPosition = false;

        if (PlayerRangeCheck(aggroRange))
        {
            ChangeState(EnemyState.Aggro);
        }
    }

    void Wander()
    {

        targetPos = new Vector2(RandomPosition(3f, -3f), RandomPosition(3f, -3f));

        Movement(targetPos);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f || (hasReachedWanderPosition == false))
        {
            //makes it get stuck on the point until it can wiggle off
            ChangeState(EnemyState.Idle);
            hasReachedWanderPosition = true;
        }




        /*
        targetPos = new Vector3(RandomPosition(3, -3f), RandomPosition(3, -3f), 0);

        oldPos = new Vector3(transform.position.x, transform.position.y, 0);

        Movement(targetPos);




        /*

        if (!isWandering)
        {
            newPosition = new Vector2(RandomPosition(5f, -5f), RandomPosition(5f, -5f));
            isWandering = true;
        }

        if (newPosition.x >= transform.position.x + .01)
        {
            Debug.Log("at position x");
        }


        previousPosition = currentPosition;

        //newPosition = new Vector2(3f, 3f);
        //newPosition = new Vector2(RandomPosition(5f, -5f), RandomPosition(5f, -5f));
        //newPosition = new Vector2(RandomPositionWhole(5, -5), RandomPositionWhole(5, -5));

        Movement(newPosition);

        //Debug.Log(newPosition);

        */
    }

    void Aggro()
    {
        Movement(targetPlayer.transform.position);
    }

    private bool PlayerRangeCheck(float distance)
    {
        if (Vector3.Distance(transform.position, targetPlayer.transform.position) < distance)
            return true;
        else
            return false;

    }

    Vector3 CurrentPosition(float xPos, float YPos)
    {
        currentPosition = new Vector3(xPos, YPos, 0);

        return currentPosition;
    }


    void DebugControls()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(currentPosition);
            Debug.Log(previousPosition);
            Debug.Log(newPosition);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeState(EnemyState.Idle);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeState(EnemyState.Aggro);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeState(EnemyState.Wander);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

}
