using UnityEngine;

public class ObjetPhysiqueDetectionSceau : MonoBehaviour
{
    public RandomMovementV2test BatmanIA;

    public float cooldown = 5f;
    private float timerCooldown;

    public GameObject flaquePrefab; 
    private bool flaqueCree = false;
    public AudioClip SonAlerte;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timerCooldown > 0)
        {
            timerCooldown -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (timerCooldown > 0) 
        {       
            return;
        }

        bool Alerte = false;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Le joueur a poussé l'objet !");
            Alerte = true;
        }

        if (Alerte)
        {
            BatmanIA.ObjetDetected(transform.position);
            audioSource.PlayOneShot(SonAlerte);
            CreationFlaque(collision.contacts[0].point);
            timerCooldown = cooldown;
        }
    }

    void CreationFlaque(Vector3 position)
    {
        if (flaqueCree)
        {
            return;
        }

        if (flaquePrefab == null)
        {
            flaqueCree = true;
            return;
        }
        
        GameObject flaque = Instantiate(flaquePrefab, position + new Vector3(0f, -0.33f, 0f), Quaternion.identity);
    
        flaqueCree = true;
    }
}
