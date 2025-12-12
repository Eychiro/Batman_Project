using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class LancementCinematicEnding : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Transform player;
    public Transform head;
    public TextMeshProUGUI InteractionText;
    public RandomMovementV2test batman;
    public Transform coffreFortArtefact;
    public Light flashLight;
    public GameObject GameoverObject;
    
    private AudioSource audioSourceFootsteps;
    private AudioSource audioSourceRespiration;

    private bool PlayerInRange = false;
    private bool stopInteraction = false;

    void Start()
    {
        audioSourceFootsteps = GetComponents<AudioSource>()[0];
        audioSourceRespiration = GetComponents<AudioSource>()[1];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            InteractionText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerInRange = false;
        InteractionText.enabled = false;
    }
    
        IEnumerator TeleportBatmanAfterCinematic()
    {
        yield return new WaitForSeconds(Convert.ToSingle(playableDirector.duration - 1));

        batman.transform.position = coffreFortArtefact.GetChild(1).position + Vector3.up;
        batman.agent.enabled = true;
        batman.GetComponent<Renderer>().enabled = true;
        batman.GetComponent<Collider>().enabled = false;
        StartCoroutine(ChargePlayer());
    }

    public IEnumerator ChargePlayer()
    {
        Vector3 startPosition = batman.transform.position;
        Vector3 targetPosition = new Vector3(player.transform.position.x, batman.transform.position.y, player.transform.position.z);

        float timeElapsed = 0f;
        float duration = 0.5f;

        yield return new WaitForSeconds(1f);

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            batman.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            timeElapsed += Time.deltaTime;

            yield return null; 
        }
        StartCoroutine(batman.GameOverSequence());
    }

    void Update()
    {
        if (PlayerInRange && !stopInteraction && Input.GetKeyDown(KeyCode.E))
        {
            stopInteraction = true;
            flashLight.enabled = false;
            InteractionText.enabled = false;

            player.position = coffreFortArtefact.GetChild(0).position;

            head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, 180, head.rotation.eulerAngles.z);
            head.GetComponent<CameraController>().ResetPos();
            
            playableDirector.Play();
            audioSourceFootsteps.Play();
            audioSourceRespiration.Play();
            batman.DisparitionForcee();

            player.GetComponent<BlockingPlayer>().LockingPlayer();

            Destroy(gameObject.GetComponent<Collider>());

            StartCoroutine(TeleportBatmanAfterCinematic());
        }
    }
}
