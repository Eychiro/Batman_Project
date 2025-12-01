using UnityEngine;

public class ObjetPhysiqueDetection : MonoBehaviour
{
    public RandomMovementV2test BatmanIA;

    public float cooldown = 5f;
    private float timerCooldown;
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
            audioSource.PlayOneShot(SonAlerte);
            BatmanIA.ObjetDetected(transform.position);
            timerCooldown = cooldown;
        }
    }
}
