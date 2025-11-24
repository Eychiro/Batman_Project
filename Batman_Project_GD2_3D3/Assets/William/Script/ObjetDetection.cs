using UnityEngine;

public class ObjetDetection : MonoBehaviour
{

    public RandomMovementV2test BatmanIA;

    public float cooldown = 3f;
    private float prochaineActivation = 0f;

    void Start()
    {
        
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
                prochaineActivation = Time.time + cooldown;
            }
        }
    }
}
