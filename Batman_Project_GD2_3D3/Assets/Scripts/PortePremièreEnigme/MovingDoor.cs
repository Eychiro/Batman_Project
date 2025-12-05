using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingDoor : MonoBehaviour
{
    public GameObject keyPadCodePorte;
    public GameObject Indice1;
    public GameObject Indice2;
    public GameObject Indice3;
    public GameObject Indice4;
    public CameraController cameraController;
    public TextMeshProUGUI _inputCode;

    public GameObject BackgroundMonologue;
    public TextMeshProUGUI monologueText;

    private Interactible _indice1;
    private Interactible _indice2;
    private Interactible _indice3;
    private Interactible _indice4;

    private bool playerInRange = false;
    private int[] requiredCode = new int[4];
    private string requiredCodeToString = "";
    private int nbrButtonsCliqued = 0;

    private AudioSource audioSourceError;

    private Transform _targetToLook;

    void Start()
    {
        audioSourceError = GetComponent<AudioSource>();
        _targetToLook = transform;
        
        keyPadCodePorte.SetActive(false);
        if (Indice1 != null)
        {
            _indice1 = Indice1.GetComponent<Interactible>();
            requiredCode[0] = _indice1.RandomNbr;
        }
        if (Indice2 != null)
        {
            _indice2 = Indice2.GetComponent<Interactible>();
            requiredCode[1] = _indice2.RandomNbr;
        }
        if (Indice3 != null)
        {
            _indice3 = Indice3.GetComponent<Interactible>();
            requiredCode[2] = _indice3.RandomNbr;
        }
        if (Indice4 != null)
        {
            _indice4 = Indice4.GetComponent<Interactible>();
            requiredCode[3] = _indice4.RandomNbr;
        }

        requiredCodeToString = string.Join("", requiredCode);
        Debug.Log(requiredCodeToString);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            cameraController.cameraLocked = false;
            keyPadCodePorte.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _inputCode.text = "";
            nbrButtonsCliqued = 0;
        }
    }

    IEnumerator RemoveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        monologueText.enabled = false;
        BackgroundMonologue.SetActive(false);
    }

    void Update()
    {
        if(playerInRange && !keyPadCodePorte.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("tu viens de cliquer !");
            
            cameraController.transform.LookAt(_targetToLook.GetComponent<Renderer>().bounds.center);
            cameraController.ResetPos();

            keyPadCodePorte.SetActive(true);
            cameraController.cameraLocked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (nbrButtonsCliqued == requiredCode.Length)
        {        // Si le code est bon
            if (_inputCode.text == requiredCodeToString)
            {
                _inputCode.text = "";
                nbrButtonsCliqued = 0;
                
                gameObject.transform.position +=  Vector3.up * 3;
                cameraController.cameraLocked = false;
                keyPadCodePorte.SetActive(false);

                BackgroundMonologue.SetActive(true);
                monologueText.enabled = true;
                monologueText.text = "Allez, allez ! Faut que je me barre d'ici !";
                StartCoroutine(RemoveTextAfterDelay(2.0f));

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else // Si le code écrit est mauvais
            {
                audioSourceError.Play();
                _inputCode.text = "";
                nbrButtonsCliqued = 0;
            }
        }
        
        if (keyPadCodePorte.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            cameraController.cameraLocked = false;
            keyPadCodePorte.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _inputCode.text = "";
            nbrButtonsCliqued = 0;
        }
    }
    
    public void ButtonPressed()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        _inputCode.text += clicked.GetComponentInChildren<TMP_Text>().text;
        nbrButtonsCliqued += 1;
    }
}
