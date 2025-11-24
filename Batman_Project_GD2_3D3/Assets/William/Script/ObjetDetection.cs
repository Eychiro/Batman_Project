using UnityEngine;

public class ObjetDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RandomMovementV2test BatmanIA;
    public MonoBehaviour playerMovementScript; // Le script de mouvement du joueur


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            if (BatmanIA != null)
            {
                BatmanIA.ObjetDetected(transform.position); 
            }

            gameObject.SetActive(false);
        }
    }
}
