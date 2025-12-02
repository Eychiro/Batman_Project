using BetterTriggers;
using TMPro;
using UnityEngine;

public class Badge : MonoBehaviour
{
    public TextMeshProUGUI emptyBadgeText;
    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    [HideInInspector] public bool badgeTaken = false;

    private bool playerInRange = false;

    void Awake()
    {        
        if (InteractionRange != null)
        {
            InteractionRange.OnTriggerEntered += OnInteractionTriggeredEnter;
            InteractionRange.OnTriggerExited += OnInteractionTriggeredExit;
        }
        if (OutlinerRange != null)
        {
            OutlinerRange.OnTriggerEntered += OnOutlinerTriggeredEnter;
            OutlinerRange.OnTriggerExited += OnOutlinerTriggeredExit;
        }
    }

    void OnInteractionTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (emptyBadgeText != null)
            {
                emptyBadgeText.enabled = true;
            }
        }
    }

    void OnInteractionTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (emptyBadgeText != null)
            {
                emptyBadgeText.enabled = false;
            }
        }
    }
    
    void OnOutlinerTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 6;
        }
    }

    void OnOutlinerTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (emptyBadgeText != null)
            {
                emptyBadgeText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (emptyBadgeText != null)
            {
                emptyBadgeText.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            badgeTaken = true;
            emptyBadgeText.enabled = false;
            Destroy(gameObject);
        }
    }
}
