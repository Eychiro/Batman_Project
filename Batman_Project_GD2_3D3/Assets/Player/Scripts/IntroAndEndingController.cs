using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroAndEndingController : MonoBehaviour
{
    public Transform Player;

    public GameObject introScreen;
    public TextMeshProUGUI textIntro;
    public string textIntroString;

    public GameObject endingScreen;
    public TextMeshProUGUI textEnding;
    public string textEndingString;

    private string textEndingFin = "FIN";

    public AudioClip crawlingSound;
    public AudioClip jumpingSound;
    public AudioClip landingSound;
    public AudioClip runningAwaySound;

    public TextMeshProUGUI initiationLampeTorche;

    public RandomMovementV2test batmanIA;

    [Header("Introduction")]
    public bool skipIntro = false;

    private BlockingPlayer _playerControllerMovement;
    private Image imageComponent;

    void Start()
    {
        _playerControllerMovement = Player.GetComponent<BlockingPlayer>();
        _playerControllerMovement.LockingPlayer();
        imageComponent = endingScreen.GetComponent<Image>();

    if (!skipIntro)
        {
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
            StartCoroutine(RevealCharactersIntro());
            StartCoroutine(AudiosPlaying());
        }
        else
        {
            _playerControllerMovement.UnlockingPlayer();
            endingScreen.SetActive(false);
            introScreen.SetActive(false);
        }
    }

    IEnumerator AudiosPlaying()
    {
        yield return new WaitForSeconds(1f);
        AudioSource.PlayClipAtPoint(crawlingSound, transform.position);

        yield return new WaitForSeconds(20f);
        AudioSource.PlayClipAtPoint(jumpingSound, transform.position);
        AudioSource.PlayClipAtPoint(landingSound, transform.position);
    }

    public IEnumerator FadeTextIntroToZeroAlpha(float t, TextMeshProUGUI i)
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

    IEnumerator RevealCharactersIntro()
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
            StartCoroutine(FadeTextIntroToZeroAlpha(3f, textIntro));
        }
    }

    public IEnumerator FadeTextEndingToZeroAlpha(float t, TextMeshProUGUI i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            
            yield return new WaitForSeconds(1f);
            
            textEnding.text = textEndingFin;
            textEnding.fontSize = 80;
            StartCoroutine(FadeTextToFill(2, textEnding));
        }

    IEnumerator RevealCharactersEnding()
    {
        int totalCharacters = textEndingString.Length;
        
        textEnding.maxVisibleCharacters = 0; 
        
        for (int i = 1; i <= totalCharacters; i++)
        {
            textEnding.maxVisibleCharacters = i; 

            yield return new WaitForSeconds(0.04f); 
        }
        textEnding.maxVisibleCharacters = totalCharacters;
        
        if (textEnding.maxVisibleCharacters == totalCharacters)
        {
            StartCoroutine(FadeTextEndingToZeroAlpha(3f, textEnding));
        }
    }

    public IEnumerator FadeImageToFill(float t, Image i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
            textEnding.text = textEndingString;
            textEnding.enabled = true;
            
            AudioSource.PlayClipAtPoint(runningAwaySound, transform.position);

            StartCoroutine(RevealCharactersEnding());
        }

    public IEnumerator FadeTextToFill(float t, TextMeshProUGUI i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
            textEnding.text = textEndingFin;
            textEnding.enabled = true;
        }


    public void InitiateEnding()
    {
        endingScreen.SetActive(true);

        _playerControllerMovement.LockingPlayer();
        batmanIA.DisparitionForcee();

        StartCoroutine(FadeImageToFill(2, imageComponent));
    }
}