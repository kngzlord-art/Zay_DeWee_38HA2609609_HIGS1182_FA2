using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase }
    public EnemyState state;

    [Header("Patrol")]
    public Transform[] waypoints;
    public float waitAtPoint = 1.5f;
    public bool loop = true;

    [Header("Player")]
    public Transform player;
    public float detectRange = 5f;
    public float loseRange = 8f;

    [Header("Movement")]
    public float chaseSpeed = 3.5f;
    public float patrolSpeed = 1.5f;

    private int index = 0;
    private float waitTimer = 0f;

    private void Start()
    {
        
        if (!player)
        {
            GameObject playerobj = GameObject.FindGameObjectWithTag("Player");

            if (playerobj)
            {
                player = playerobj.transform;
            }
        }


        if (state == EnemyState.Patrol && waypoints != null && waypoints.Length > 0)
        {
            goTo(index);
        }
    }

    private void Update()
    {
        
        if (player)
        {
            float dist = Vector3.Distance(transform.position, player.position);

            
            if (state != EnemyState.Chase && dist <= detectRange)
            {
                state = EnemyState.Chase;
            }

            
            else if (state == EnemyState.Chase && dist > loseRange)
            {
                if (waypoints != null && waypoints.Length > 0)
                {
                    state = EnemyState.Patrol;
                    goTo(index);
                }
                else
                {
                    state = EnemyState.Idle;
                }
            }
        }

        
        switch (state)
        {
            case EnemyState.Idle:
                updateIdle();
                break;

            case EnemyState.Patrol:
                updatePatrol();
                break;

            case EnemyState.Chase:
                updateChase();
                break;
        }
    }

    

    public void updateIdle()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                return;
            }

            index++;

            if (index >= waypoints.Length)
            {
                if (loop)
                {
                    index = 0;
                }
                else
                {
                    index = waypoints.Length - 1;
                    return;
                }
            }

            state = EnemyState.Patrol;
            goTo(index);
        }
    }

    

    public void updatePatrol()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            state = EnemyState.Idle;
            return;
        }

        Transform targetWaypoint = waypoints[index];

        if (targetWaypoint == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            patrolSpeed * Time.deltaTime
        );

        Vector3 direction = targetWaypoint.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.1f)
        {
            state = EnemyState.Idle;
            waitTimer = waitAtPoint;
        }
    }
    public void updateChase()
    {
        if (!player)
        {
            return;
        }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

       
        Vector3 direction = player.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

   

    public void goTo(int i)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (i < 0 || i >= waypoints.Length)
        {
            return;
        }

        index = i;
    }

    

    public void Die()
    {
        Destroy(gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}