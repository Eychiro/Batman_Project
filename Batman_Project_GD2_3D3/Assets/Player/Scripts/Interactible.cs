using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public GameObject emptyPourDésactiver;

    private bool playerInRange = false;

    void Start()
    {
        emptyPourDésactiver.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            Debug.Log("hello tu es dedans");
            emptyPourDésactiver.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("aurevoir !");
            emptyPourDésactiver.SetActive(false);
        }

    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("tu viens d'appuyer sur le bouton !");
        }
    }
}
