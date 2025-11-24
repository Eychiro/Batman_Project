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
        Cooldown,
        ObjetDetecPoursuite
    }
    
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;
    private Vector3 trapTargetPosition;
    public Transform player;
    public float normalDetection = 10f;
    public float lightDetection = 100f;

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

            case Etat.ObjetDetecPoursuite:
                UpdateObjDetecPoursuite();
                CheckForDetection();
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
        Debug.Log("Début de la poursuite (light)");
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
    

    void SwitchToObjetDetecPoursuite()
    {
        etatActuel = Etat.ObjetDetecPoursuite;
        etatTimer = 60f;
        Debug.Log("Le piège a été activé, Batman arrive !");
    }

    void UpdatePoursuite()
    {
        float distancePlayer = Vector3.Distance(transform.position, player.position);
        
        if (distancePlayer <= normalDetection)
        {
            agent.SetDestination(player.position);

        }

        if (CheckIfLightIsVisible())
        {
            SwitchToPoursuiteLight();
            return;
        }
        
        if (etatTimer <= 0)
        {
            SwitchToCooldown();
        }
    }

    void UpdatePoursuiteLight()
    {
        agent.SetDestination(player.position);

        if (CheckIfLightIsVisible() == false)
        {
            float distancePlayer = Vector3.Distance(transform.position, player.position);

            if (distancePlayer <= normalDetection)
            {
                SwitchToPoursuite(); 
            }
            else
            {
                SwitchToRecherche();
            }
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

    void UpdateObjDetecPoursuite()
    {
        agent.SetDestination(trapTargetPosition);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            SwitchToRecherche();
        }
        
        if (etatTimer <= 0)
        {
            SwitchToRecherche();
        }
    }

    void CheckForDetection()
    {
        float distancePlayer = Vector3.Distance(transform.position, player.position);
        
        if (CheckIfLightIsVisible())
        {
            SwitchToPoursuiteLight();
            return;
        }

        if (distancePlayer <= normalDetection)
        {
            SwitchToPoursuite();
        }
    }

    bool CheckIfLightIsVisible()
    {
        if (playerScript != null && playerScript.isFlashlightOn)
        {
            float distancePlayer = Vector3.Distance(transform.position, player.position);

            if (distancePlayer <= lightDetection)
            {
                return true;
            }
        }
        return false;
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

    public void ObjetDetected(Vector3 trapPosition)
    {
        trapTargetPosition = trapPosition;
        SwitchToObjetDetecPoursuite();
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

