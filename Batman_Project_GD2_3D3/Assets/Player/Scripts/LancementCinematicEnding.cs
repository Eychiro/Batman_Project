using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LancementCinematicEnding : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Transform player;
    public Transform head;
    public TextMeshProUGUI InteractionText;
    public RandomMovementV2test batman;

    private bool PlayerInRange = false;

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
    
    void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {  
            head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, 180, head.rotation.eulerAngles.z);
            head.GetComponent<CameraController>().ResetPos();
            
            playableDirector.Play();

            batman.DisparitionForcee();

            player.GetComponent<BlockingPlayer>().LockingPlayer();

            Destroy(gameObject.GetComponent<Collider>());
        }
    }
}
