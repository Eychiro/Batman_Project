using UnityEngine;

public class ObjetDetection : MonoBehaviour
{

    public RandomMovementV2test BatmanIA;

    public float cooldown = 3f;
    private float prochaineActivation = 0f;
    public AudioClip SonAlerte;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= prochaineActivation)
        { 
            if (BatmanIA != null)
            {
                BatmanIA.ObjetDetected(transform.position);
                audioSource.PlayOneShot(SonAlerte);
                prochaineActivation = Time.time + cooldown;
            }
        }
    }
}
