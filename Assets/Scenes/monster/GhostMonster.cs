using UnityEngine;
using UnityEngine.AI;
using Supercyan.FreeSample; // For Joystickscript

public class GhostMonster : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public float chaseDistance = 5f;
    public float stunDuration = 2f;

    private NavMeshAgent agent;
    private int currentPatrol = 0;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0].position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!chasing && distanceToPlayer < chaseDistance)
        {
            chasing = true;
        }
        else if (chasing && distanceToPlayer > chaseDistance * 1.5f)
        {
            chasing = false;
        }

        if (chasing)
        {
            agent.destination = player.position;
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f && patrolPoints.Length > 0)
        {
            currentPatrol = (currentPatrol + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrol].position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Joystickscript playerScript = other.GetComponent<Joystickscript>();
            if (playerScript != null)
            {
                Debug.Log("Stun: Player has been stunned by the ghost!");
                playerScript.Stun(stunDuration);
            }
        }
    }
}