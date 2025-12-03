using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool PauseVisible = false;
    public GameObject Canva;
    public AudioSource Son;

    void Start()
    {
        PauseVisible = false;
        Canva.SetActive(false);

        //PauseVisible = true;
        //this.gameObject.SetActive(true);
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
        Son.Play();
    }

    public void PauseDeactiver()
    {
        Cursor.lockState = CursorLockMode.Locked ;
        Cursor.visible = false;
        Time.timeScale = 1f;
        PauseVisible = false;
        Canva.SetActive(false);
        Son.Play();
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
