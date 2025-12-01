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

    private IEnumerator MoveDoor()
    {
        float time = 0f;
        Vector3 targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 3, transform.localPosition.z);

        while(time <= durationOpeningDoor)
        {
            float t = time / durationOpeningDoor;
            time += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, t);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    public void CoroutineMoveDoor()
    {
        StartCoroutine(MoveDoor());
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
