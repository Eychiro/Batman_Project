using System.Collections;
using TMPro;
using UnityEngine;

public class IntroAndEndingController : MonoBehaviour
{
    public Transform Player;

    public GameObject introScreen;
    public TextMeshProUGUI textIntro;
    public string textIntroString;

    public GameObject endingScreen;
    public TextMeshProUGUI textEnding;
    public string textEndingString;
    
    public AudioClip crawlingSound;
    public AudioClip jumpingSound;
    public AudioClip landingSound;

    public TextMeshProUGUI initiationLampeTorche;

    private BlockingPlayer _playerControllerMovement;

    void Start()
    {
        _playerControllerMovement = Player.GetComponent<BlockingPlayer>();
        _playerControllerMovement.LockingPlayer();
        
        if (initiationLampeTorche != null)
        {
            initiationLampeTorche.ForceMeshUpdate(true);
            initiationLampeTorche.enabled = false;
        }

        if (textIntro != null && textEnding != null)
        {
            textIntro.text = textIntroString;
            textIntro.enabled = true;

            textEnding.text = textEndingString;
            textEnding.enabled = false;
            endingScreen.SetActive(false);
        }        
        StartCoroutine(RevealCharacters());
        StartCoroutine(AudiosPlaying());
    }

    IEnumerator AudiosPlaying()
    {
        yield return new WaitForSeconds(1f);
        AudioSource.PlayClipAtPoint(crawlingSound, transform.position);

        yield return new WaitForSeconds(20f);
        AudioSource.PlayClipAtPoint(jumpingSound, transform.position);
        AudioSource.PlayClipAtPoint(landingSound, transform.position);
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            introScreen.SetActive(false);
            _playerControllerMovement.UnlockingPlayer();
            initiationLampeTorche.enabled = true;
        }

    IEnumerator RevealCharacters()
    {
        // Nombre total de caractères dans le texte à révéler
        int totalCharacters = textIntroString.Length;
        
        textIntro.maxVisibleCharacters = 0; 
        
        // Boucle pour révéler chaque caractère un par un
        for (int i = 1; i <= totalCharacters; i++)
        {
            // Définit le nombre maximum de caractères visibles
            textIntro.maxVisibleCharacters = i; 

            // Attend le délai spécifié avant de passer au caractère suivant
            yield return new WaitForSeconds(0.04f); 
        }
        textIntro.maxVisibleCharacters = totalCharacters;
        
        if (textIntro.maxVisibleCharacters == totalCharacters)
        {
            StartCoroutine(FadeTextToZeroAlpha(3f, textIntro));
        }
    }
}