using System.Collections;
using NUnit.Framework;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.SceneManagement; utile pour relancer le niveau

public class RandomMovementV2testV12 : MonoBehaviour
{
    private enum Etat
    {
        Recherche,
        Poursuite,
        PoursuiteLight,
        Cooldown,
        ObjetDetecPoursuite,
        Disparu
    }
    
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;
    private Vector3 objetTargetPosition;
    public Transform player;
    public float normalDetection = 10f;
    public float lightDetection = 100f;

    public float minTempsPoursuite = 1f;
    public float maxTempsPoursuite = 10f;
    public float minTempsCooldown = 1f;
    public float maxTempsCooldown = 10f;

    public float minTempsAvantDisparition = 90f;
    public float maxTimeAvantDisparition = 150f;
    public float disparitionDuree = 60f;

    public Transform spawnPointA;
    public Transform spawnPointB;
    
    private Etat etatActuel;
    private float etatTimer;
    private float disparitionCountdownTimer;

    public float mortDistance = 2.5f;
    public float tempsJumpscare = 0.1f; 
    private bool jeuFini = false;

    public float stopDistance = 1.0f;

    public bool estCache = false;
    private bool detecteAvantCache = false;

    private bool isDormant = true;
    
    private WillCameraController playerScript;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerScript = player.GetComponent<WillCameraController>();

        isDormant = true;
        etatActuel = Etat.Disparu;

        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    void ResetDisappearCountdown()
    {
        disparitionCountdownTimer = Random.Range(minTempsAvantDisparition, maxTimeAvantDisparition);
        Debug.Log("Prochaine disparition de Batman dans: " + disparitionCountdownTimer + " secondes.");
    }

    void Update()
    {
        if (jeuFini || isDormant) 
        return;

        if (etatActuel != Etat.Disparu)
        {
            VerifMort();
        }
        
        if (etatTimer > 0)
        {
            etatTimer -= Time.deltaTime;
        }

        if (etatActuel != Etat.Disparu)
        {
            if (disparitionCountdownTimer > 0)
            {
                disparitionCountdownTimer -= Time.deltaTime;
            }
            if (disparitionCountdownTimer <= 0)
            {
                SwitchToDisparu();
                return;
            }
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

            case Etat.Disparu:
                UpdateDisparu();
                break;   
        }
    }
    
    void SwitchToDisparu()
    {
        etatActuel = Etat.Disparu;
        etatTimer = disparitionDuree;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        agent.enabled = false; 
        
        Debug.Log("Batman a disparu !");
    }

    void SwitchToPoursuite()
    {
        etatActuel = Etat.Poursuite;
        etatTimer = Random.Range(minTempsPoursuite, maxTempsPoursuite);
        agent.stoppingDistance = stopDistance;
        detecteAvantCache = true;
        Debug.Log("Début de la poursuite pour : " + etatTimer + " secondes");
    }

    void SwitchToPoursuiteLight()
    {
        etatActuel = Etat.PoursuiteLight;
        agent.stoppingDistance = stopDistance;
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
        agent.stoppingDistance = 0.1f;
        detecteAvantCache = false;
        Debug.Log("Retour en patrouille active");
    }
    

    void SwitchToObjetDetecPoursuite()
    {
        etatActuel = Etat.ObjetDetecPoursuite;
        etatTimer = 60f;
        Debug.Log("L'objet a été activé, Batman arrive !");
    }

    void UpdateDisparu()
    {
        if (etatTimer <= 0)
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            agent.enabled = true; 
            Vector3 positionApparition = ChoixSpawn();
            agent.Warp(positionApparition);
            ResetDisappearCountdown();
            SwitchToRecherche();
            
            Debug.Log("Batman est de retour!");
        }
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
        agent.SetDestination(objetTargetPosition);

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
        if ((estCache && !detecteAvantCache) || !EstDansLaZoneActuelle(player.position)) 
        return;
        
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

    public void ObjetDetected(Vector3 objetPosition)
    {
        if (etatActuel == Etat.Disparu || !EstDansLaZoneActuelle(objetPosition)) 
        return;
        
        objetTargetPosition = objetPosition;
        SwitchToObjetDetecPoursuite();
    }

    Vector3 ChoixSpawn()
{
    if (spawnPointA == null || spawnPointB == null || player == null)
    {
        Debug.LogError("Erreur : Points de spawn ou référence au joueur manquant(s).");
        return transform.position; 
    }

    float distA = Vector3.Distance(spawnPointA.position, player.position);
    float distB = Vector3.Distance(spawnPointB.position, player.position);

    if (distA > distB)
    {
        return spawnPointA.position;
    }
    else
    {
        return spawnPointB.position;
    }
}

    void VerifMort()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= mortDistance && (!estCache || detecteAvantCache))
        {
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        if (jeuFini)
        yield break;

        jeuFini = true;
        agent.isStopped = true; 
        agent.velocity = Vector3.zero;

        if (playerScript != null) 
        playerScript.enabled = false;   

        yield return new WaitForSeconds(tempsJumpscare);
        Time.timeScale = 0f;
        Debug.Log("Normalement le jeu s'arrête là");    
    }

    bool EstDansLaZoneActuelle(Vector3 targetPosition)
{
    float distanceCentre = Vector3.Distance(targetPosition, centrePoint.position);
    
    if (distanceCentre > range + 5f) 
    {
        return false;
    }
    return true;
}

    public void ActiverEtChangerZone(Transform nouvCentre, float nouvRange, Transform nouvSpawnA, Transform nouvSpawnB, float delaiFX)
    {
        centrePoint = nouvCentre;
        range = nouvRange;
        spawnPointA = nouvSpawnA;
        spawnPointB = nouvSpawnB;

            if (isDormant)
        {
            isDormant = false;
        }

        StartCoroutine(SequenceApparition(delaiFX));
    }

    IEnumerator SequenceApparition(float delai)
    {
        SwitchToDisparu(); 
        yield return new WaitForSeconds(delai);

        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        agent.enabled = true; 

        Vector3 positionApparition = ChoixSpawn();
        agent.Warp(positionApparition);

        ResetDisappearCountdown();
        SwitchToRecherche();
    }

    public void DisparitionForcee()
{
    if (etatActuel != Etat.Disparu)
    {
        SwitchToDisparu();
    }
    
    etatTimer = float.MaxValue; 
    disparitionCountdownTimer = -1f;
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

        
        //Mort
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, mortDistance);
    }
}