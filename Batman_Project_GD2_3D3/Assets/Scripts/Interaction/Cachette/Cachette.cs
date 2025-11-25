using TMPro;
using UnityEngine;

public class Cachette : MonoBehaviour
{
    public TextMeshProUGUI emptyPourDésactiver;
    public Transform Player;
    public CameraController cameraController;
    public Light flashLight;
    public string textItem = "Appuyer sur E pour rentrer dans la cachette";

    private bool playerInRange = false;
    private bool _isHidden = false;
    private MovementController _playerMovement;

    void Start()
    {
        _playerMovement = Player.GetComponent<MovementController>();
        
        if (emptyPourDésactiver != null)
        {
            emptyPourDésactiver.text = textItem;
            emptyPourDésactiver.ForceMeshUpdate(true); 
            emptyPourDésactiver.enabled = false;
        }

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
        if (playerInRange && !_isHidden && Input.GetKeyDown(KeyCode.E))
        {
            _isHidden = true;
            Player.transform.position = transform.GetChild(0).position;
            cameraController.transform.rotation = transform.GetChild(0).rotation;
            cameraController._camera.localRotation = Quaternion.identity;
            flashLight.gameObject.SetActive(false);
            _playerMovement.movementLocked = true;
            emptyPourDésactiver.text = "Appuyer sur E pour sortir de la cachette";

            cameraController.cameraLocked = true;
            return;
        }

        if (playerInRange && _isHidden && Input.GetKeyDown(KeyCode.E))
        {
            _isHidden = false;
            cameraController.cameraLocked = false;
            Player.transform.position = transform.GetChild(1).position;
            cameraController.ResetPos();
            _playerMovement.movementLocked = false;
            emptyPourDésactiver.text = "Appuyer sur E pour rentrer dans la cachette";
        }
    }
}