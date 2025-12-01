using TMPro;
using UnityEngine;

public class Badge : MonoBehaviour
{
    public TextMeshProUGUI emptyPourDésactiver;
    
    [HideInInspector] public bool badgeTaken = false;

    private bool playerInRange = false;

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            badgeTaken = true;
            emptyPourDésactiver.enabled = false;
            Destroy(gameObject);
        }
    }
}
