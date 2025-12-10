using TMPro;
using UnityEngine;
using BetterTriggers;
using System.Collections;

public class Levier : MonoBehaviour
{
    public TextMeshProUGUI textLevier;
    public GameObject coffreFortArtefact;
    public GameObject porteEnding;

    public Trigger InteractionRange;
    public Trigger OutlinerRange;

    public RandomMovementV2test batmanIA;

    public Transform nouveauCentrePoint;
    public float nouvelleRange = 50f;
    public Transform nouveauSpawnA;
    public Transform nouveauSpawnB;
    public float delaiAvantApparition = 3.0f;

    public AudioClip LevierSliding;

    public GameObject cinematicEnding;

    [HideInInspector] public bool porteEndingOuverte = false;

    private bool playerInRange = false;
    private bool _isUsed = false;

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
        if (collider.CompareTag("Player") && !_isUsed)
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
        if (collider.CompareTag("Player") && !_isUsed)
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

    IEnumerator RotateLevier()
    {
        float t = 0;

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.Euler(45, 0, 0), t);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(MoveCoffreFort());
        batmanIA.ActiverEtChangerZone(nouveauCentrePoint, nouvelleRange, nouveauSpawnA, nouveauSpawnB, delaiAvantApparition);
    }

    IEnumerator MoveCoffreFort()
    {
        float t = 0;
        Vector3 targetPosition = new Vector3(coffreFortArtefact.transform.localPosition.x, coffreFortArtefact.transform.localPosition.y - 4, coffreFortArtefact.transform.localPosition.z);

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            coffreFortArtefact.transform.localPosition = Vector3.Lerp(coffreFortArtefact.transform.localPosition, targetPosition, t);

            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(RotatePorteCoffreFort());
        StartCoroutine(RotatePorteEnding());
    }

    IEnumerator RotatePorteCoffreFort()
    {
        float t = 0;
        Transform porteCoffreFort = coffreFortArtefact.transform.GetChild(0);

        cinematicEnding.SetActive(true);

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            porteCoffreFort.localRotation = Quaternion.Lerp(porteCoffreFort.localRotation, Quaternion.Euler(0, 90, 0), t);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator RotatePorteEnding()
    {
        float t = 0;
        Transform porteCoffreFort = porteEnding.transform;

        porteEndingOuverte = true;

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            porteCoffreFort.localRotation = Quaternion.Lerp(porteCoffreFort.localRotation, Quaternion.Euler(0, -60, 0), t);
            yield return new WaitForSeconds(0.01f);
        }
    }


    void Update()
    {
        if (playerInRange && !_isUsed && Input.GetKeyDown(KeyCode.E))
        {
            AudioSource.PlayClipAtPoint(LevierSliding, transform.position);

            _isUsed = true;
            textLevier.enabled = false;
            gameObject.layer = 0;

            StartCoroutine(RotateLevier());
        }
    }
}
