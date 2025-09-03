using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyType : MonoBehaviour
{
    private GameObject target;
    private Animator _animator;
    public bool pathfind = false;
    public bool patrol = false;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public float speed = 1f;
    
    [Header("Smart Patrol Settings")]
    public float chaseRange = 5f; // How close player needs to be to start chasing
    public float patrolWidth = 3f; // How far from the patrol line the player can be
    public float returnDistance = 8f; // How far player can go before enemy gives up chase
    
    private bool isChasing = false;
    private Vector3 lastPatrolTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        target = GameObject.FindGameObjectWithTag("Player");

        if (patrol)
        {
            agent.autoBraking = false;
            GotoNextPoint();
        }
        _animator = GetComponent<Animator>();
        _animator.SetBool("isWalking", true); // might need ot change if enemies r idle
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;
        lastPatrolTarget = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        if (pathfind)
        {
            if (!target.activeInHierarchy)
            {
                if (points.Length > 0)
                {
                    agent.SetDestination(points[0].position);
                }
            }
            else
            {
                agent.SetDestination(target.transform.position);
            }
            return;
        }

        // Smart patrol behavior
        if (patrol && target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
            
            if (!isChasing)
            {
                // Check if player is close enough and within patrol area to start chasing
                if (distanceToPlayer <= chaseRange && IsPlayerWithinPatrolArea())
                {
                    isChasing = true;
                    agent.autoBraking = true; // Enable braking for better chase behavior
                }
                else
                {
                    // Continue normal patrol
                    if (agent.remainingDistance < 0.5f)
                    {
                        GotoNextPoint();
                    }
                }
            }
            else
            {
                // Currently chasing - check if we should give up
                if (distanceToPlayer > returnDistance || !IsPlayerWithinPatrolArea())
                {
                    // Give up chase and return to patrol
                    isChasing = false;
                    agent.autoBraking = false;
                    ReturnToPatrol();
                }
                else
                {
                    // Continue chasing
                    agent.SetDestination(target.transform.position);
                }
            }
        }
        // Original patrol behavior for when there's no target
        else if (patrol)
        {
            if (agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
    }

    bool IsPlayerWithinPatrolArea()
    {
        if (!target.activeInHierarchy) return false;
        if (points.Length < 2) return true; // If no proper patrol path, allow chasing

        // Find the closest point on the patrol path to the player
        Vector3 closestPoint = GetClosestPointOnPatrolPath(target.transform.position);
        
        // Check if player is within the patrol width
        float distanceFromPath = Vector3.Distance(target.transform.position, closestPoint);
        return distanceFromPath <= patrolWidth;
    }

    Vector3 GetClosestPointOnPatrolPath(Vector3 playerPos)
    {
        Vector3 closestPoint = points[0].position;
        float closestDistance = Vector3.Distance(playerPos, closestPoint);

        // Check distance to each patrol point
        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(playerPos, points[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = points[i].position;
            }
        }

        // Also check distances to line segments between patrol points
        for (int i = 0; i < points.Length; i++)
        {
            int nextIndex = (i + 1) % points.Length;
            Vector3 pointOnLine = GetClosestPointOnLineSegment(
                points[i].position, 
                points[nextIndex].position, 
                playerPos
            );
            
            float distance = Vector3.Distance(playerPos, pointOnLine);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = pointOnLine;
            }
        }

        return closestPoint;
    }

    Vector3 GetClosestPointOnLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLength = lineDirection.magnitude;
        lineDirection.Normalize();

        Vector3 pointDirection = point - lineStart;
        float dot = Vector3.Dot(pointDirection, lineDirection);

        // Clamp to line segment
        dot = Mathf.Clamp(dot, 0f, lineLength);

        return lineStart + lineDirection * dot;
    }

    void ReturnToPatrol()
    {
        // Find the closest patrol point and resume patrol from there
        int closestPointIndex = 0;
        float closestDistance = Vector3.Distance(transform.position, points[0].position);

        for (int i = 1; i < points.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, points[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPointIndex = i;
            }
        }

        destPoint = closestPointIndex;
        GotoNextPoint();
    }

    // Visual debugging in Scene view
    void OnDrawGizmosSelected()
    {
        if (points == null || points.Length < 2) return;

        // Draw patrol path
        Gizmos.color = Color.blue;
        for (int i = 0; i < points.Length; i++)
        {
            int nextIndex = (i + 1) % points.Length;
            if (points[i] != null && points[nextIndex] != null)
            {
                Gizmos.DrawLine(points[i].position, points[nextIndex].position);
            }
        }

        // Draw patrol area (width visualization)
        Gizmos.color = Color.cyan;
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] != null)
            {
                Gizmos.DrawWireSphere(points[i].position, patrolWidth);
            }
        }

        // Draw chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Draw return distance when chasing
        if (isChasing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, returnDistance);
        }
    }
}