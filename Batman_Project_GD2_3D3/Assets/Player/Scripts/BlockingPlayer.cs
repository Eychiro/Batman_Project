using UnityEngine;

public class BlockingPlayer : MonoBehaviour
{
    public Transform Player;
    public CameraController cameraController;
    private MovementController _playerMovement;


    void Start()
    {
        _playerMovement = Player.GetComponent<MovementController>();
    }


    public void LockingPlayer()
    {
        _playerMovement.movementLocked = true;
        cameraController.cameraLocked = true;
    }
    
    public void UnlockingPlayer()
    {
        _playerMovement.movementLocked = false;
        cameraController.cameraLocked = false;
    }
}
