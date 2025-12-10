using UnityEngine;

public class PlayerHeartBeat : MonoBehaviour
{
    [HideInInspector] public Transform BatmanPos;

    private AudioSource heartBeatAudioSource;

    void Start()
    {
        heartBeatAudioSource = GetComponent<AudioSource>();        
    }

    void Update()
    {
        if (BatmanPos == null)
        {
            if (heartBeatAudioSource != null && heartBeatAudioSource.isPlaying)
            {
                heartBeatAudioSource.Stop();
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, BatmanPos.position);

        if (distance < 30f)
        {
            if (heartBeatAudioSource != null && !heartBeatAudioSource.isPlaying)
            {
                heartBeatAudioSource.Play();
            }
            heartBeatAudioSource.volume = Mathf.Clamp01((30f - distance) / 30f);
            heartBeatAudioSource.pitch = 1f + (30f - distance) / 40f;
        }
        else
        {
            if (heartBeatAudioSource != null && heartBeatAudioSource.isPlaying)
            {
                heartBeatAudioSource.Stop();
            }
        }
    }
}
