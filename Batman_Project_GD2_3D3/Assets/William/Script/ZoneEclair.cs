using UnityEngine;
using System.Collections;

public class ZoneEclair : MonoBehaviour
{
    public LightningEffect lightningScript; 

    private bool dejaActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (lightningScript == null) 
        {
            return; 
        }

        if (other.CompareTag("Player") && !dejaActive)
        {
            dejaActive = true;
            
            lightningScript.StartCoroutine(lightningScript.TriggerAndResumeSequence()); 
            
            enabled = false; 
        }
    }
}