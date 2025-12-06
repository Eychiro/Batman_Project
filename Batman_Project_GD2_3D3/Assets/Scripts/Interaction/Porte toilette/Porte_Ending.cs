using System.Collections;
using TMPro;
using UnityEngine;

public class Porte_Ending : MonoBehaviour
{
    public TextMeshProUGUI PorteSortieText;
    public Levier levier;
    public IntroAndEndingController introAndEndingController;

    private bool playerInRange = false;

    void Start()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levier.porteEndingOuverte)
        {
            playerInRange = true;
            
            if (PorteSortieText != null)
            {
                PorteSortieText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && levier.porteEndingOuverte)
        {
            playerInRange = false;
            
            if (PorteSortieText != null)
            {
                PorteSortieText.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PorteSortieText.enabled = false;
            introAndEndingController.InitiateEnding();
        }
    }
}
