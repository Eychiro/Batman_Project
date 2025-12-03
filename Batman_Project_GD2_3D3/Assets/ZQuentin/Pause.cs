using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    private bool PauseVisible = false;
    public GameObject Canva;
    public AudioSource Son;

    public TextMeshProUGUI VolumeText;
    public Slider SliderVolume;
    private float Volume;

    void Start()
    {
        Time.timeScale = 1f;
        PauseVisible = false;
        Canva.SetActive(false);
        Volume = AudioListener.volume * 100f;
        VolumeText.text = $"Volume: {Volume}%";
        SliderVolume.value = Volume;
        Debug.Log(Volume);

        //PauseActiver();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            BoutonPause();
        }
    }


    public void BoutonPause()
    {
        if (PauseVisible)
        {
            PauseDeactiver();
        }
        else
        {
            PauseActiver();
        }
    }
    
    public void PauseActiver()
    {
        Cursor.lockState = CursorLockMode.None ;
        Cursor.visible = true;
        Time.timeScale = 0f;
        PauseVisible = true;
        Canva.SetActive(true);
        Son.pitch = 1 ;
        Son.Play();
        AudioListener.volume = Volume/100;
    }

    public void PauseDeactiver()
    {
        Cursor.lockState = CursorLockMode.Locked ;
        Cursor.visible = false;
        Time.timeScale = 1f;
        PauseVisible = false;
        Canva.SetActive(false);
        Son.pitch = 0.9f ;
        Son.Play();
    }

    public void VolumeUpdate()
    {
        Volume = SliderVolume.value;
        //VolumeText.text = "Volume:" + Volume.ToString();
        VolumeText.text = $"Volume: {Volume}%";
        AudioListener.volume = Volume/100;
        Debug.Log(Volume);
    }

    public void Continuer()
    {
        PauseDeactiver();
    }

    public void Recommencer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quitter()
    {
        SceneManager.LoadScene("ZQuentin/MainMenu");
    }
}
