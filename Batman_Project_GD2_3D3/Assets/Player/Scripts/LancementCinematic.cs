using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LancementCinematic : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Transform player;
    public Transform head;

    IEnumerator UnlockingMovementAfterCinematic()
    {
        yield return new WaitForSeconds(Convert.ToSingle(playableDirector.duration));

        player.GetComponent<BlockingPlayer>().UnlockingPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, 90, head.rotation.eulerAngles.z);
            head.GetComponent<CameraController>().ResetPos();
            
            playableDirector.Play();
            player.GetComponent<BlockingPlayer>().LockingPlayer();

            Destroy(gameObject.GetComponent<Collider>());

            StartCoroutine(UnlockingMovementAfterCinematic());
        }
    }
}
