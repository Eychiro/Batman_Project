using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    //inicialisation
    public Image logo;
    public GameObject Bouttons;
    public Image EcranNoir;


    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void LancerGameOver()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        var elapsed = 0f;
        var FadeTime = 1.5f;

        EcranNoir.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        while (elapsed < FadeTime)
        {
            elapsed += Time.deltaTime;

            var ratio = elapsed / FadeTime;

            EcranNoir.color = Color32.Lerp(new Color32(0,0,0,255),new Color32(0,0,0,0), ratio);

            yield return null;
        }

        EcranNoir.gameObject.SetActive(false);
    }

    public void PressedRejouer()
    {
        Debug.Log("Restart");
        EcranNoir.gameObject.SetActive(true);
        EcranNoir.color = new Color32(0,0,0,255);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void PressedQuitter()
    {
        Debug.Log("Quitter");
        EcranNoir.gameObject.SetActive(true);
        EcranNoir.color = new Color32(0,0,0,255);
        SceneManager.LoadScene("ZQuentin/MainMenu");
    }
}
