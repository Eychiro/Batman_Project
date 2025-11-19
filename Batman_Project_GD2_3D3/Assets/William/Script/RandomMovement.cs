using NUnit.Framework;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    public Transform player;
    public float normalDetection = 5f;
    public float lightDetection = 15f;
    public bool isTooClose;
    public bool isLightVisible;
    
    private WillCameraController playerScript;
    
    void Start()
    {
      agent = GetComponent<NavMeshAgent>();
      if (player != null)
        {
            playerScript = player.GetComponent<WillCameraController>();
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool isTooClose = distanceToPlayer <= normalDetection;
        
        bool isLightVisible = false;
        if (playerScript != null && playerScript.isFlashlightOn)
        {
            if (distanceToPlayer <= lightDetection)
            {
                isLightVisible = true;
            }
        }

        if (isTooClose || isLightVisible)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                Debug.DrawRay(point,Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}
