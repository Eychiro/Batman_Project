using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Groupe
    public GameObject GroupeTitre;
    public GameObject GroupeBoutonsPrincipaux;
    public GameObject GroupeOption;
    public GameObject GroupeCredit;
    public Image EcranNoir;
    public Image THUNDER;

    public AudioSource Musique;
    public AudioSource SonEclair;
    public AudioSource SonClic;

    public GameObject MainCamera;

    public TextMeshProUGUI VolumeText;
    public Slider SliderVolume;
    private float Volume = 50f;

    public float FadeTime = 2f;

    
    void Start()
    {   
        AudioListener.volume = Volume/100;
        Musique.time = 4f;
        Musique.Play();
        EcranNoir.gameObject.SetActive(true);
        EcranNoir.color = new Color32(0,0,0,255);
        GroupeTitre.SetActive(false);
        THUNDER.gameObject.SetActive(false);
        GroupeBoutonsPrincipaux.SetActive(false);
        GroupeOption.SetActive(false);
        GroupeCredit.SetActive(false);
        StartCoroutine(Introduction());
    }


    IEnumerator Introduction()
    {
        var elapsed = 0f;

        yield return new WaitForSeconds(1f);

        while (elapsed < FadeTime)
        {
            elapsed += Time.deltaTime;

            var ratio = elapsed / FadeTime;

            EcranNoir.color = Color32.Lerp(new Color32(0,0,0,255),new Color32(0,0,0,100), ratio);
            MainCamera.transform.position = Vector3.Lerp(new Vector3(0,0,-10), new Vector3(0,0,-5) , ratio);

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < FadeTime)
        {
            elapsed += Time.deltaTime;

            var ratio = elapsed / FadeTime;

            EcranNoir.color = Color32.Lerp(new Color32(0,0,0,100),new Color32(0,0,0,255), ratio);
            MainCamera.transform.position = Vector3.Lerp(new Vector3(0,0,-5), new Vector3(0,0,0) , ratio);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ThunderEffect());
        EcranNoir.color = new Color32(0,0,0,0);
        EcranNoir.gameObject.SetActive(false);
        GroupeTitre.SetActive(true);
    }

    public void PressedCredit()
    {
        GroupeBoutonsPrincipaux.SetActive(false);
        GroupeCredit.SetActive(true);
        SonClic.Play();
    }

    public void PressedTitre()
    {
        GroupeTitre.SetActive(false);
        GroupeBoutonsPrincipaux.SetActive(true);
        SonClic.Play();
    }

    public void PressedJouer()
    {
        StartCoroutine(JeuLancer());
        SonClic.Play();
    }

    public void PressedOption()
    {
        Debug.Log("OPTION");
        PressedVolume();
        GroupeBoutonsPrincipaux.SetActive(false);
        GroupeOption.SetActive(true);
        SonClic.Play();
        AudioListener.volume = Volume/100;
    }

    public void PressedQuitter()
    {
        Debug.Log("QUIT");
        Application.Quit();
        SonClic.Play();
    }

    public void PressedRetour()
    {
        Debug.Log("Retour");
        GroupeOption.SetActive(false);
        GroupeCredit.SetActive(false);
        GroupeBoutonsPrincipaux.SetActive(true);
        SonClic.Play();
    }

    public void PressedVolume()
    {
        Volume = SliderVolume.value;
        //VolumeText.text = "Volume:" + Volume.ToString();
        VolumeText.text = $"Volume: {Volume}%";
        AudioListener.volume = Volume/100;
        Debug.Log(Volume);
    }

    IEnumerator ThunderEffect()
    {
        THUNDER.gameObject.SetActive(true);
        var elapsed = 0f;
        FadeTime = 0.25f;
        SonEclair.Play();

        while (elapsed < FadeTime)
        {
            elapsed += Time.deltaTime;

            var ratio = elapsed / FadeTime;

            THUNDER.color = Color32.Lerp(new Color32(255,255,255,255),new Color32(255,255,255,0), ratio);

            yield return null;
        }
        THUNDER.gameObject.SetActive(false);
    }

    IEnumerator JeuLancer()
    {
        var elapsed = 0f;
        FadeTime = 2f;
        
        EcranNoir.gameObject.SetActive(true);
        EcranNoir.color = new Color32(0,0,0,0);
        GroupeBoutonsPrincipaux.SetActive(false);

        while (elapsed < FadeTime)
        {
            elapsed += Time.deltaTime;

            var ratio = elapsed / FadeTime;

            Musique.volume = Mathf.Lerp(1,0,ratio);
            EcranNoir.color = Color32.Lerp(new Color32(0,0,0,0),new Color32(0,0,0,255), ratio);
            MainCamera.transform.position = Vector3.Lerp(new Vector3(0,0,0), new Vector3(0,0,2.5f) , ratio);

            yield return null;
        }
        Musique.volume = 0f;

        yield return new WaitForSeconds(2f);

        Debug.Log("PLAY");
        SceneManager.LoadScene("William/SampleSceneWill");
    }
}
