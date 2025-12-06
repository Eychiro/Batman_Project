using TMPro;
using UnityEngine;

public class Cachette : MonoBehaviour
{
    public TextMeshProUGUI cachetteText;
    public Transform Player;
    public CameraController cameraController;
    public Light flashLight;
    public string textItem = "Appuyer sur E pour rentrer dans la cachette";
    public RandomMovementV2test BatmanAI;

    private bool playerInRange = false;
    private bool _isHidden = false;
    private MovementController _playerMovement;

    void Start()
    {
        _playerMovement = Player.GetComponent<MovementController>();
        
        if (cachetteText != null)
        {
            cachetteText.text = textItem;
            cachetteText.ForceMeshUpdate(true);
            cachetteText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (cachetteText != null)
            {
                cachetteText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (cachetteText != null)
            {
                cachetteText.enabled = false;
            }
        }
    }

    void Update()
    {
        if (playerInRange && !_isHidden && Input.GetKeyDown(KeyCode.E))
        {
            _isHidden = true;

            if (BatmanAI != null) 
            BatmanAI.estCache = true;

            Player.transform.position = transform.GetChild(0).position;
            cameraController.transform.rotation = transform.GetChild(0).rotation;
            cameraController._camera.localRotation = Quaternion.identity;
            flashLight.enabled = false;
            _playerMovement.movementLocked = true;
            cachetteText.text = "Appuyer sur E pour sortir de la cachette";

            cameraController.cameraLocked = true;
            return;
        }

        if (playerInRange && _isHidden && Input.GetKeyDown(KeyCode.E))
        {
            _isHidden = false;

            if (BatmanAI != null) 
            BatmanAI.estCache = false;
            
            cameraController.cameraLocked = false;
            Player.transform.position = transform.GetChild(1).position;
            cameraController.ResetPos();
            _playerMovement.movementLocked = false;
            cachetteText.text = "Appuyer sur E pour rentrer dans la cachette";
        }
    }
}