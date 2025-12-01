using System.Collections;
using TMPro;
using UnityEngine;

public class Porte_Toilette : MonoBehaviour
{
    public TextMeshProUGUI emptyPourDésactiver;

    private bool playerInRange = false;
    private bool _isClosed = true;

    void Start()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isClosed)
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
        if (other.CompareTag("Player") && _isClosed)
        {
            playerInRange = false;
            
            if (emptyPourDésactiver != null)
            {
                emptyPourDésactiver.enabled = false;
            }
        }
    }

    IEnumerator MovePorte()
    {
        float time = 0;

        while(time <= 1)
        {
            time += Time.deltaTime;
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.Euler(0, 90, 0), time);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    void Update()
    {
        if (playerInRange && _isClosed && Input.GetKeyDown(KeyCode.E))
        {
            _isClosed = false;
            emptyPourDésactiver.enabled = false;

            StartCoroutine(MovePorte());
        }
    }
}
