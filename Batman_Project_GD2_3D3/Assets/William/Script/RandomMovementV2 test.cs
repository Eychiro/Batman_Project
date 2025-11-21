using NUnit.Framework;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovementV2test : MonoBehaviour
{
    private enum Etat
    {
        Recherche,
        Poursuite,
        PoursuiteLight,
        Cooldown
    }
    
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    public Transform player;
    public float normalDetection = 10f;
    public float lightDetection = 100f;
    public bool isLightVisible = false;


    public float minTempsPoursuite = 1f;
    public float maxTempsPoursuite = 10f;
    public float minTempsCooldown = 1f;
    public float maxTempsCooldown = 10f;

    private Etat etatActuel;
    private float etatTimer;
    
    private WillCameraController playerScript;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerScript = player.GetComponent<WillCameraController>();

        SwitchToRecherche();
    }

    void Update()
    {
        if (etatTimer > 0)
        {
            etatTimer -= Time.deltaTime;
        }

        switch (etatActuel)
        {
            case Etat.Recherche:
                UpdateRecherche();
                CheckForDetection();
                break;

            case Etat.Poursuite:
                UpdatePoursuite();
                break;

            case Etat.PoursuiteLight:
                UpdatePoursuiteLight();
                break;

            case Etat.Cooldown:
                UpdateRecherche();
                CheckCooldownEnd();
                break;
        }
    }
    
    void SwitchToPoursuite()
    {
        etatActuel = Etat.Poursuite;
        etatTimer = Random.Range(minTempsPoursuite, maxTempsPoursuite);
        Debug.Log("Début de la poursuite pour : " + etatTimer + " secondes");
    }

    void SwitchToPoursuiteLight()
    {
        etatActuel = Etat.PoursuiteLight;
        etatTimer = 10000f;
        Debug.Log("Début de la poursuite pour : " + etatTimer + " secondes");
    }

    void SwitchToCooldown()
    {
        etatActuel = Etat.Cooldown;
        etatTimer = Random.Range(minTempsCooldown, maxTempsCooldown);
        Debug.Log("Fatigué ! Repos pour : " + etatTimer + " secondes");
    }

    void SwitchToRecherche()
    {
        etatActuel = Etat.Recherche;
        etatTimer = 0;
        Debug.Log("Retour en patrouille active");
    }

    void UpdatePoursuite()
    {
        agent.SetDestination(player.position);

        if (etatTimer <= 0)
        {
            SwitchToCooldown();
        }
    }

    void UpdatePoursuiteLight()
    {
        agent.SetDestination(player.position);

        if (isLightVisible == false)
        {
            SwitchToCooldown();
        }
    }

    void UpdateRecherche()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
            }
        }
    }

    void CheckForDetection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isClose = distanceToPlayer <= normalDetection;
        
        bool isLightVisible = false;
        if (playerScript != null && playerScript.isFlashlightOn)
        {
            if (distanceToPlayer <= lightDetection) isLightVisible = true;
        }

        if (isClose)
        {
            SwitchToPoursuite();
        }

        if (isLightVisible)
        {
            SwitchToPoursuiteLight();
        }
    }

    void CheckCooldownEnd()
    {
        if (etatTimer <= 0)
        {
            SwitchToRecherche();
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

    
    //Juste pour vérifier la détection, pour playtest et balance
    void OnDrawGizmosSelected()
    {
        //Normal
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, normalDetection);

        //Lumière
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightDetection);
    }
}

