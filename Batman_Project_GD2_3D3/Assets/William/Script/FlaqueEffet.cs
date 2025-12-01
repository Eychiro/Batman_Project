using UnityEngine;

public class FlaqueEffet : MonoBehaviour
{
    public float slowFactor = 0.5f;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MovementController playerMovement = other.GetComponent<MovementController>();
            
            if (playerMovement != null)
            {
                playerMovement.speed = playerMovement.speed / 2;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MovementController playerMovement = other.GetComponent<MovementController>();
            
            if (playerMovement != null)
            {
                playerMovement.speed = playerMovement.speed * 2;
            }
        }
    }
}
