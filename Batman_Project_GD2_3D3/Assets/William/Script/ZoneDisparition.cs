using UnityEngine;

public class ZoneDisparition : MonoBehaviour
{
    public RandomMovementV2test batmanIA;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (batmanIA != null)
            {
                batmanIA.DisparitionForcee();
                Destroy(gameObject); 
            }
        }
    }
}
