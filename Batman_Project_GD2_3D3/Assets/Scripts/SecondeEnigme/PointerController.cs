using UnityEngine;
 
public class PointerController : MonoBehaviour
{
    public Transform pointA; // Reference point de départ
    public Transform pointB; // Reference point de fin
    public RectTransform safeZone; // Reference safe Zone
    public float moveSpeed = 200f; // Vitesse du mouvement de la barre
    public CoffreFort coffreFort;
    
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private int successCount = 0;
    private Vector3 originalSafeZoneWidth;

    [HideInInspector] public bool _alreadyOpened = false;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();

        targetPosition = pointB.position;
        originalSafeZoneWidth = safeZone.localScale;
        
        // Largeur de la safe zone
        float safeZoneWidth = safeZone.rect.width;
        
        // Position aléatoire entre min et max, en laissant de la marge pour la largeur
        float randomX = Random.Range(pointA.position.x + safeZoneWidth / 2f, pointB.position.x - safeZoneWidth / 2f);
        
        Vector3 newSafeZone = new Vector3(randomX, safeZone.position.y, safeZone.position.z);
        safeZone.position = newSafeZone;
    }
 
    void Update()
    {
        // pointer va vers la target position
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);
 
        // si le point est atteint, alors changer la direction
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
        }
 
        // check input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }
 
    private void SuccessModifier()
    {
        // Largeur de la safe zone
        float newSafeZoneWidth = safeZone.rect.width;
        
        // Position aléatoire entre min et max, en laissant de la marge pour la largeur
        float randomX = Random.Range(pointA.position.x + newSafeZoneWidth / 2f, pointB.position.x - newSafeZoneWidth / 2f);
        
        Vector3 newSafeZone = new Vector3(randomX, safeZone.position.y, safeZone.position.z);
        safeZone.position = newSafeZone;

        safeZone.localScale = new Vector3(safeZone.localScale.x - 0.2f, safeZone.localScale.y, safeZone.localScale.z);
        moveSpeed += 200;
    }

    public void LeavingModifier()
    {
        if (_alreadyOpened)
        {
            safeZone.localScale = originalSafeZoneWidth;

            float newSafeZoneWidth = safeZone.rect.width;
        
            // Position aléatoire entre min et max, en laissant de la marge pour la largeur
            float randomX = Random.Range(pointA.position.x + newSafeZoneWidth / 2f, pointB.position.x - newSafeZoneWidth / 2f);
        
            Vector3 newSafeZone = new Vector3(randomX, safeZone.position.y, safeZone.position.z);
            safeZone.position = newSafeZone;

            moveSpeed = 200;
            successCount = 0;
        }
        else
        _alreadyOpened = true;
    }
    
    private void CheckSuccess()
    {
        float newSafeZoneWidth;
        float randomX;
        Vector3 newSafeZone;

        // Check if pointer is in safe Zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!");
            successCount += 1;
            if (successCount < 1)
            {
                SuccessModifier();
            }
            else
            {
            // Reussite du coffre fort
            transform.parent.gameObject.SetActive(false);
            coffreFort._playerMovement.movementLocked = false;
            coffreFort.cameraController.cameraLocked = false;
            coffreFort.coffreFortText.enabled = false;
            coffreFort._isOpened = true;
            coffreFort.CoroutineMoveDoor();
            }
        }
        else
        {
            Debug.Log("Fail!");
            if (successCount > 0)
            {
                successCount -= 1;
                moveSpeed -= 200;
                safeZone.localScale = new Vector3(safeZone.localScale.x + 0.2f, safeZone.localScale.y, safeZone.localScale.z);
                
                newSafeZoneWidth = safeZone.rect.width;
                
                // Position aléatoire entre min et max, en laissant de la marge pour la largeur
                randomX = Random.Range(pointA.position.x + newSafeZoneWidth / 2f, pointB.position.x - newSafeZoneWidth / 2f);
                
                newSafeZone = new Vector3(randomX, safeZone.position.y, safeZone.position.z);
                safeZone.position = newSafeZone;
            }
            newSafeZoneWidth = safeZone.rect.width;

            randomX = Random.Range(pointA.position.x + newSafeZoneWidth / 2f, pointB.position.x - newSafeZoneWidth / 2f);
            
            newSafeZone = new Vector3(randomX, safeZone.position.y, safeZone.position.z);
            safeZone.position = newSafeZone;
        }
    }
}
