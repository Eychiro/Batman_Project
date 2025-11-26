using TMPro;
using UnityEngine;

public class CoffreFort : MonoBehaviour
{
    public GameObject emptyUiCoffreFort;
    public TextMeshProUGUI coffreFortText;
    public Transform Player;
    public CameraController cameraController;
    public string textItem = "Appuyer sur E pour tenter d'ouvrir le coffre";
    public PointerController pointerController;

    private bool playerInRange = false;
    private bool _uiAffichee = false;
    private MovementController _playerMovement;

    void Start()
    {
        _playerMovement = Player.GetComponent<MovementController>();
        
        if (coffreFortText != null)
        {
            coffreFortText.text = textItem;
            coffreFortText.ForceMeshUpdate(true);
            coffreFortText.enabled = false;
        }

        if (emptyUiCoffreFort == true)
        {
            emptyUiCoffreFort.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (coffreFortText != null)
            {
                coffreFortText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (coffreFortText != null)
            {
                coffreFortText.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerInRange && !_uiAffichee && Input.GetKeyDown(KeyCode.E))
        {
            pointerController.LeavingModifier();
            _uiAffichee = true;
            emptyUiCoffreFort.SetActive(true);
            _playerMovement.movementLocked = true;
            cameraController.cameraLocked = true;
            coffreFortText.text = "Appuyer sur E pour annuler l'ouverture";
            return;
        }

        if (playerInRange && _uiAffichee && Input.GetKeyDown(KeyCode.E))
        {
            _uiAffichee = false;
            emptyUiCoffreFort.SetActive(false);
            _playerMovement.movementLocked = false;
            cameraController.cameraLocked = false;
            coffreFortText.text = "Appuyer sur E pour tenter d'ouvrir le coffre";
        }
    }
}
