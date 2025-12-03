using UnityEngine;

public class ZoneApparitionCouloir : MonoBehaviour
{
    public BatmanCouloirIA batmanIA;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (batmanIA != null)
            {
                batmanIA.ApparaitreEtCommencer();
                Destroy(gameObject); 
            }
        }
    }
}
