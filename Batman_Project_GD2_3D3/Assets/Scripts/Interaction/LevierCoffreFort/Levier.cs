using TMPro;
using UnityEngine;
using BetterTriggers;
using System.Collections;

public class Levier : MonoBehaviour
{
    public TextMeshProUGUI textLevier;

    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    private bool playerInRange = false;

    void Awake()
    {        
        if (InteractionRange != null)
        {
            InteractionRange.OnTriggerEntered += OnInteractionTriggeredEnter;
            InteractionRange.OnTriggerExited += OnInteractionTriggeredExit;
        }
        if (OutlinerRange != null)
        {
            OutlinerRange.OnTriggerEntered += OnOutlinerTriggeredEnter;
            OutlinerRange.OnTriggerExited += OnOutlinerTriggeredExit;
        }
    }

    void Start()
    {
        if (textLevier != null)
        {
            textLevier.ForceMeshUpdate(true); 
            textLevier.enabled = false;
        }
    }

    void OnInteractionTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (textLevier != null)
            {
                textLevier.enabled = true;
            }
        }
    }

    void OnInteractionTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (textLevier != null)
            {
                textLevier.enabled = false;
            }
        }
    }

    void OnOutlinerTriggeredEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 6;
        }
    }

    void OnOutlinerTriggeredExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.layer = 0;
        }
    }

    IEnumerator MoveCoffreFort()
        {
            yield return new WaitForSeconds(0.5f);
            // Animation du coffre Fort ici
        }

    IEnumerator MoveLevier()
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime;
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.Euler(transform.parent.rotation.x - 45, transform.parent.rotation.y, transform.parent.rotation.z), t);
                yield return null;
            }
        }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MoveLevier());
        }
    }
}
