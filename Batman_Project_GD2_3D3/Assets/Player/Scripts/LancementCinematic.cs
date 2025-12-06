using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LancementCinematic : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Transform player;

    // private float duration;

    // void Start()
    // {
    //     duration = Convert.ToSingle(playableDirector.duration);
    // }

    IEnumerator UnlockingMovementAfterCinematic()
    {
        yield return new WaitForSeconds(Convert.ToSingle(playableDirector.duration));

        player.GetComponent<BlockingPlayer>().UnlockingPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playableDirector.Play();
            player.GetComponent<BlockingPlayer>().LockingPlayer();

            Destroy(gameObject.GetComponent<Collider>());

            StartCoroutine(UnlockingMovementAfterCinematic());
        }
    }
}
