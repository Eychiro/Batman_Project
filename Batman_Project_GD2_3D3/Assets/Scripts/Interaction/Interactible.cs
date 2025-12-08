using System.Collections;
using BetterTriggers;
using TMPro;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public TextMeshProUGUI emptyPourDésactiver;
    public GameObject backgroundNoteIndice;
    public TextMeshProUGUI noteIndiceEnigme1;

    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    public string TextItem = "Appuyer sur E pour interagir";
    public int RandomNbr {get;private set;}

    public string prefixePhrase;
    public string suffixePhrase;

    private bool playerInRange = false;
    private bool _noteAffichee = false;

    private Transform _targetToLook;
    private MovingDoor _variables;

    public int GetRandomInt()
    {
        return Random.Range(1, 9);
    }

    void Awake()
    {

        RandomNbr = GetRandomInt();
        
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
        _targetToLook = transform;
        _variables = GetComponentInParent<MovingDoor>();

        if (emptyPourDésactiver != null)
        {
            emptyPourDésactiver.text = TextItem;
            emptyPourDésactiver.ForceMeshUpdate(true); 
            emptyPourDésactiver.enabled = false;
        }

        if (noteIndiceEnigme1 != null)
        {
            noteIndiceEnigme1.ForceMeshUpdate(true); 
            noteIndiceEnigme1.enabled = false;
            backgroundNoteIndice.SetActive(false);
        }
    }

    void OnInteractionTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            
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
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = false;
            
                if (_noteAffichee)
                {
                    _variables.cameraController.cameraLocked = false;
                    
                    noteIndiceEnigme1.enabled = false;
                    backgroundNoteIndice.SetActive(false);
                    _noteAffichee = false;
                }
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

        if (playerInRange && !_noteAffichee && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(RandomNbr);

            if (noteIndiceEnigme1 != null)
            {
                _variables.cameraController.transform.LookAt(_targetToLook.GetComponent<Renderer>().bounds.center);
                _variables.cameraController.ResetPos();
                _variables.cameraController.cameraLocked = true;

                _noteAffichee = true;
                noteIndiceEnigme1.enabled = true;
                emptyPourDésactiver.enabled = false;
                backgroundNoteIndice.SetActive(true);
                noteIndiceEnigme1.text = prefixePhrase + " " + RandomNbr + " " + suffixePhrase;
                return;
            }
        }

        if (playerInRange && _noteAffichee && Input.GetKeyDown(KeyCode.E))
            {
                _variables.cameraController.cameraLocked = false;
                Debug.Log("tu veux te barrer !");
                noteIndiceEnigme1.enabled = false;
                emptyPourDésactiver.enabled = true;
                backgroundNoteIndice.SetActive(false);
                _noteAffichee = false;
            }
    }
}