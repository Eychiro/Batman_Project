using BetterTriggers;
using TMPro;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public TextMeshProUGUI emptyPourDésactiver;
    public Trigger InteractionRange;
    public Trigger OutlinerRange;
    public string TextItem = "Appuyer sur E pour interagir";
    
    private bool playerInRange = false;
    [HideInInspector] public int randomNbr;

    public int GetRandomInt()
    {
        return Random.Range(1, 9);
    }

    void Awake()
    {
        randomNbr = GetRandomInt();
        
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
    
    void Start()
    {
        if (emptyPourDésactiver != null)
        {
            emptyPourDésactiver.text = TextItem;
            emptyPourDésactiver.ForceMeshUpdate(true); 
            emptyPourDésactiver.enabled = false;
        }
    }

    void OnInteractionTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("hello tu es dans le point d'interaction");
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = true;
            }
        }
    }

    void OnInteractionTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("aurevoir !");
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = false;
            }
        }
    }
    
    void OnOutlinerTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 6;
            Debug.Log("l'objet devient lumière !");
        }
    }

    void OnOutlinerTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 0;
            Debug.Log("L'objet devient ténèbres !");
        }
    }
        
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("tu viens d'appuyer sur le bouton !");
            Debug.Log(randomNbr);
        }
    }
}