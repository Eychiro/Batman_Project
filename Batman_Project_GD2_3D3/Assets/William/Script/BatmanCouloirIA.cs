using System.Collections;
using UnityEngine;

public class BatmanCouloirIA : MonoBehaviour
{
    public GameObject GameoverObject;
    public float tempsJumpscare = 0.1f; 
    private bool jeuFini = false;
    private CameraController playerScript;
    public Transform player;

    public float vitesseDeplacement = 1.0f;
    public Vector3 positionStandby;
    public Vector3 positionDebut;
    public Vector3 positionCible;
    private bool estArrive = false; 
    private bool estActif = false;

    public MovementController movementController;

    public bool IsAgentActive
    {
        get { return estActif && !estArrive; }
    }

    void Start()
    {
        this.transform.position = positionStandby;
        
        playerScript = player.GetComponent<CameraController>();
    }
    
    void Update()
    {
        if (estActif && !jeuFini)
        {
            DeplacerBatman();
        }
    }

    public void ApparaitreEtCommencer()
    {
        if (estActif) return;

        this.transform.position = positionDebut;
        estActif = true;
    }

    void DeplacerBatman()
    {
        if (estArrive)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            positionCible,
            vitesseDeplacement * Time.deltaTime
        );
        
        if (Vector3.Distance(transform.position, positionCible) < 0.001f)
        {
            estArrive = true;
            estActif = false;
            Debug.Log("Batman a atteint sa position cible.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        if (jeuFini)
        yield break;

        jeuFini = true;

        if (playerScript != null) 
        playerScript.enabled = false;
        movementController.enabled = false;   

        yield return new WaitForSeconds(tempsJumpscare);
        Debug.Log("Normalement le jeu s'arrête là");
        GameObject.Find("Pause Object").SetActive(false);  
        GameoverObject.GetComponent<GameOver>().LancerGameOver();  
    }
}
