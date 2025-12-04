using UnityEngine;

public class HeartBeatController : MonoBehaviour
{
    public BatmanCouloirIA batmanCouloir; 
    public RandomMovementV2test batmanClassique;

    public PlayerHeartBeat heartBeatScript;

    void Update()
    {
        if (heartBeatScript == null) return;
        
        Transform nouvelleCible = null;

        if (batmanClassique != null && batmanClassique.IsAgentActive)
        {
            nouvelleCible = batmanClassique.transform;
        }
        else if (batmanCouloir != null && batmanCouloir.IsAgentActive)
        {
            nouvelleCible = batmanCouloir.transform;
        }
        
        if (heartBeatScript.BatmanPos != nouvelleCible)
        {
            heartBeatScript.BatmanPos = nouvelleCible;
        }
    }
}
