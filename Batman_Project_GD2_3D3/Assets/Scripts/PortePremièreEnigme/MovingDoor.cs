using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingDoor : MonoBehaviour
{
    public BlockingPlayer player;

    public GameObject keyPadCodePorte;
    public GameObject Indice1;
    public GameObject Indice2;
    public GameObject Indice3;
    public GameObject Indice4;
    public CameraController cameraController;
    public TextMeshProUGUI _inputCode;

    public TextMeshProUGUI TextInteraction;

    private Interactible _indice1;
    private Interactible _indice2;
    private Interactible _indice3;
    private Interactible _indice4;

    private bool playerInRange = false;
    private int[] requiredCode = new int[4];
    private string requiredCodeToString = "";
    private int nbrButtonsCliqued = 0;

    private Transform _targetToLook;

    void Start()
    {

        if (TextInteraction != null)
        {
            TextInteraction.ForceMeshUpdate(true); 
            TextInteraction.enabled = false;
        }

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
            TextInteraction.enabled = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            TextInteraction.enabled = false;

            player.UnlockingPlayer();

            keyPadCodePorte.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _inputCode.text = "";
            nbrButtonsCliqued = 0;
        }
    }

    IEnumerator MovePortes()
    {
        float doorOpenDuration = 20f;
        float time = 0;

        while(time <= 1)
        {
            time += Time.deltaTime / doorOpenDuration;

            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0, 0), time);
            transform.GetChild(1).localRotation = Quaternion.Lerp(transform.GetChild(1).localRotation, Quaternion.Euler(0, 180, 0), time);
            yield return new WaitForSeconds(0.01f);
        }
    }


    void Update()
    {
        if(playerInRange && !keyPadCodePorte.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            player.LockingPlayer();
            cameraController.transform.LookAt(_targetToLook.GetComponent<Renderer>().bounds.center);
            cameraController.ResetPos();

            keyPadCodePorte.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (nbrButtonsCliqued == requiredCode.Length)
        {        // Si le code est bon
            if (_inputCode.text == requiredCodeToString)
            {
                Destroy(GetComponents<Collider>()[1]);
                playerInRange = false;
                TextInteraction.enabled = false;

                _inputCode.text = "";
                nbrButtonsCliqued = 0;

                player.UnlockingPlayer();

                StartCoroutine(MovePortes());

                cameraController.cameraLocked = false;
                keyPadCodePorte.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                GetComponents<AudioSource>()[0].Play();
            }
            else // Si le code écrit est mauvais
            {
                GetComponents<AudioSource>()[1].Play();
                _inputCode.text = "";
                nbrButtonsCliqued = 0;
            }
        }
        
        if (keyPadCodePorte.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            player.UnlockingPlayer();
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
