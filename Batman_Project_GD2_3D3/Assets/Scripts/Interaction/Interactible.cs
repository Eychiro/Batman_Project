using BetterTriggers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public GameObject emptyPourDésactiver;
    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    private bool playerInRange = false;
    private Outline _outline;

    void Start()
    {
        emptyPourDésactiver.SetActive(false);
        _outline = gameObject.AddComponent<Outline>();
        _outline.enabled = false;
    }

    private void Awake()
    {
        // J'ajoute la fonction Enter et Exit pour "InteractionRange" qui gère l'interaction avec les objets
        if (InteractionRange != null)
        {
            InteractionRange.OnTriggerEntered += OnInteractionTriggeredEnter;
            InteractionRange.OnTriggerExited += OnInteractionTriggeredExit;
        }
        // J'ajoute la fonction Enter et Exit pour "OutlinerRange" qui gère l'effet d'outline de l'objet
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

            Debug.Log("hello tu es dans le point d'interaction");
            emptyPourDésactiver.SetActive(true);
        }
    }

    void OnInteractionTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("aurevoir !");
            emptyPourDésactiver.SetActive(false);
        }
    }

    void OnOutlinerTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _outline.enabled = true;
            _outline.OutlineWidth = 5.0f;
            Debug.Log("l'objet devient lumière !");
        }
    }

    void OnOutlinerTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _outline.enabled = false;
            Debug.Log("L'objet devient ténèbres !");
        }
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("tu viens d'appuyer sur le bouton !");
        }
    }
}
