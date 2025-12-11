using TMPro;
using UnityEngine;
using BetterTriggers;
using System.Collections;

public class AffichageEcriteaux : MonoBehaviour
{
    public BlockingPlayer player;

    public GameObject AffichageEcriteau;
    public TextMeshProUGUI textEcriteau;
    public TextMeshProUGUI textInteraction;

    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    public AudioClip LevierSliding;

    public string TexteSurEcriteau;

    private bool playerInRange = false;
    private bool _ecriteauAffichee = false;

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

    void Start()
    {
        if (textEcriteau != null)
        {
            textEcriteau.ForceMeshUpdate(true); 
            textEcriteau.enabled = false;

            textInteraction.ForceMeshUpdate(true); 
            textInteraction.enabled = false;
        }
    }

    void OnInteractionTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (textEcriteau != null)
            {
                textEcriteau.enabled = true;
                textInteraction.enabled = true;
            }
        }
    }

    void OnInteractionTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (textEcriteau != null)
            {
                textEcriteau.enabled = false;
                textInteraction.enabled = false;
            }

            if (AffichageEcriteau.activeSelf)
            {
                AffichageEcriteau.SetActive(false);
                player.UnlockingPlayer();
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

    void Update()
    {
        if (playerInRange && !_ecriteauAffichee && Input.GetKeyDown(KeyCode.E))
        {
            AffichageEcriteau.SetActive(true);
            textEcriteau.text = TexteSurEcriteau;
            _ecriteauAffichee = true;
            player.LockingPlayer();
            //AudioSource.PlayClipAtPoint(LevierSliding, transform.position);

            gameObject.layer = 0;
            return;
        }

        if (playerInRange && _ecriteauAffichee && Input.GetKeyDown(KeyCode.E))
        {
            AffichageEcriteau.SetActive(false);
            textEcriteau.text = TexteSurEcriteau;
            _ecriteauAffichee = false;
            player.UnlockingPlayer();
            //AudioSource.PlayClipAtPoint(LevierSliding, transform.position);

            gameObject.layer = 6;
        }
    }
}
