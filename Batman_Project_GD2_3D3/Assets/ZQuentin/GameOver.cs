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
    public Image Jumpscare;


    void Start()
    {
        this.gameObject.SetActive(false);

        //debug
        //LancerGameOver();
    }

    public void LancerGameOver()
    {
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None ;
        Cursor.visible = true;
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        var elapsed = 0f;
        var FadeTime = 1f;
        var ScaleJump = 1f;
        var rotationPrep = 0f;

        EcranNoir.gameObject.SetActive(true);
        Jumpscare.gameObject.SetActive(true);  

        while (elapsed < FadeTime)
        {
            elapsed += 0.1f;
            
            Jumpscare.transform.Rotate(Vector3.forward * rotationPrep *-1);
            rotationPrep = Random.Range(-40.0f, 40.0f);
            Jumpscare.transform.Rotate(Vector3.forward * rotationPrep);
            ScaleJump = Random.Range(150,200);
            Jumpscare.transform.localScale = new Vector3(ScaleJump/100,ScaleJump/100,ScaleJump/100);
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }

        Jumpscare.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        
        elapsed = 0f;
        FadeTime = 1.5f;

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
