using TMPro;
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

    private Interactible _indice1;
    private Interactible _indice2;
    private Interactible _indice3;
    private Interactible _indice4;

    private bool playerInRange = false;
    private int[] requiredCode = new int[4];
    private string requiredCodeToString = "";
    private int nbrButtonsCliqued = 0;

    private AudioSource audioSourceError;

    void Start()
    {
        audioSourceError = GetComponent<AudioSource>();
        
        keyPadCodePorte.SetActive(false);
        if (Indice1 != null)
        {
            _indice1 = Indice1.GetComponent<Interactible>();
            requiredCode[0] = _indice1.randomNbr;
        }
        if (Indice2 != null)
        {
            _indice2 = Indice2.GetComponent<Interactible>();
            requiredCode[1] = _indice2.randomNbr;
        }
        if (Indice3 != null)
        {
            _indice3 = Indice3.GetComponent<Interactible>();
            requiredCode[2] = _indice3.randomNbr;
        }
        if (Indice4 != null)
        {
            _indice4 = Indice4.GetComponent<Interactible>();
            requiredCode[3] = _indice4.randomNbr;
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
    
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E) || playerInRange && Input.GetMouseButtonDown(0))
        {
            Debug.Log("tu viens de cliquer !");
            keyPadCodePorte.SetActive(true);
            cameraController.cameraLocked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        if (nbrButtonsCliqued == requiredCode.Length)
        {        // Si le code est bon
            if (_inputCode.text == requiredCodeToString)
            {
                _inputCode.text = "";
                nbrButtonsCliqued = 0;
                gameObject.transform.position +=  Vector3.up * 2;
                cameraController.cameraLocked = false;
                keyPadCodePorte.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else // Si le code écrit est mauvais
            {
                audioSourceError.Play();
                _inputCode.text = "";
                nbrButtonsCliqued = 0;
                return;
            }
        }
        
        if (keyPadCodePorte.activeSelf && Input.GetKeyDown(KeyCode.Escape))
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
