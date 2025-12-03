using System.Collections;
using UnityEngine;

public class BatmanCouloirIA : MonoBehaviour
{
    public GameObject GameoverObject;
    public float mortDistance = 2.5f;
    public float tempsJumpscare = 0.1f; 
    private bool jeuFini = false;
    private CameraController playerScript;
    public Vector3 positionStandby; //position déterminée dans le niveau, en dehors de tout contact pour éviter collision
    public Transform player;



    void Start()
    {
        playerScript = player.GetComponent<CameraController>();
        this.transform.position = positionStandby;

        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        VerifMort();
    }

    void VerifMort()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= mortDistance)
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

        yield return new WaitForSeconds(tempsJumpscare);
        Debug.Log("Normalement le jeu s'arrête là");
        GameObject.Find("Pause Object").SetActive(false);  
        GameoverObject.GetComponent<GameOver>().LancerGameOver();  
    }
}
