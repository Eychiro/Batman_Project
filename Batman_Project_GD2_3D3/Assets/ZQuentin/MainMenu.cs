using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Groupe
    public GameObject GroupeTitre;
    public GameObject GroupeBoutonsPrincipaux;
    public GameObject GroupeOption;

    public GameObject VolumeText;
    private float Volume;

    
    void Start()
    {
        GroupeTitre.SetActive(true);
        GroupeBoutonsPrincipaux.SetActive(false);
        GroupeOption.SetActive(false);
    }

    public void PressedTitre()
    {
        GroupeTitre.SetActive(false);
        GroupeBoutonsPrincipaux.SetActive(true);
    }

    public void PressedJouer()
    {
        GroupeBoutonsPrincipaux.SetActive(false);
        SceneManager.LoadScene("SampleSceneWill");
        Debug.Log("PLAY");
    }

    public void PressedOption()
    {
        Debug.Log("OPTION");
        GroupeBoutonsPrincipaux.SetActive(false);
        GroupeOption.SetActive(true);
    }

    public void PressedQuitter()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void PressedRetour()
    {
        Debug.Log("Retour");
        GroupeOption.SetActive(false);
        GroupeBoutonsPrincipaux.SetActive(true);
    }

    public void PressedVolume()
    {
        Volume = 10;
    }
}
