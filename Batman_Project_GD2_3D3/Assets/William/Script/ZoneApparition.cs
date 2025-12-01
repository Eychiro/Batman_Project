using UnityEngine;

public class ZoneApparition : MonoBehaviour
{
    public RandomMovementV2test batmanIA;

    // Tout est à modifier en fonction de la zone ici
    public Transform nouveauCentrePoint;
    public float nouvelleRange = 50f; // J'ai mis 50 au pif
    public Transform nouveauSpawnA;
    public Transform nouveauSpawnB;
    public float delaiAvantApparition = 3.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (batmanIA != null)
            {
                batmanIA.ActiverEtChangerZone(nouveauCentrePoint, nouvelleRange, nouveauSpawnA, nouveauSpawnB, delaiAvantApparition);
                Destroy(gameObject); 
            }
        }
    }
}
