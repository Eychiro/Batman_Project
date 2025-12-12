using System.Collections;
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
    public float durationOpeningDoor = 3f;

    [HideInInspector] public MovementController _playerMovement;
    [HideInInspector] public bool _isOpened = false;

    private bool playerInRange = false;
    private bool _uiAffichee = false;

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
        if (other.CompareTag("Player")&& !_isOpened)
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
        if (other.CompareTag("Player") && !_isOpened)
        {
            playerInRange = false;
            
            if (coffreFortText != null)
            {
                coffreFortText.enabled = false;
            }
        }
    }

    private IEnumerator MoveDoors()
    {
        float doorOpenDuration = 20f;
        float time = 0;

        while(time <= 1)
        {
            time += Time.deltaTime / doorOpenDuration;

            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -95, 0), time);
            transform.GetChild(1).localRotation = Quaternion.Lerp(transform.GetChild(1).localRotation, Quaternion.Euler(0, 115, 0), time);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    public void CoroutineMoveDoor()
    {
        StartCoroutine(MoveDoors());
    }

    void Update()
    {
        if (playerInRange && !_uiAffichee && !_isOpened && Input.GetKeyDown(KeyCode.E))
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
