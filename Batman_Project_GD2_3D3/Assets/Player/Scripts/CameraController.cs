using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CameraController : MonoBehaviour
{
    public Transform _camera;
    public Transform hand;
    public Light flashLight;
    public float cameraSensitivity = 200.0f;
    public float cameraAcceleration = 5.0f;
    [HideInInspector] public bool isFlashlightOn = false;
    public ChangementCouleur changementCouleurScript;

    private float rotationX;
    private float rotationY;
    private AudioSource flashlightSwitch;

    public bool cameraLocked = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        flashlightSwitch = GetComponent<AudioSource>();
        flashLight.enabled = false;
    }

    void Update()
    {
        if (cameraLocked)
        return;
        rotationX += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        hand.localRotation = Quaternion.Euler(-rotationX, rotationY, 0);

        ActivateLight();

        // Mouvement réaliste qui met du délai à la camera pour suivre le mouvement //
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, rotationY, 0), cameraAcceleration * Time.deltaTime);
        _camera.localRotation = Quaternion.Lerp(_camera.localRotation, Quaternion.Euler(-rotationX, 0, 0), cameraAcceleration * Time.deltaTime);
    }

    public void ActivateLight()
    {
        if (Input.GetMouseButtonDown(1))
        {
            flashlightSwitch.Play();
            flashLight.enabled = !flashLight.enabled;
            isFlashlightOn = flashLight.enabled;

            if (changementCouleurScript != null)
            {
                changementCouleurScript.StopAllFlashEffects();
            }

            Debug.Log("Lampe allumée : " + isFlashlightOn);
        }
    }

    public void ResetPos()
    {
        rotationX = 0;
        rotationY = transform.localRotation.eulerAngles.y;
        _camera.localRotation = Quaternion.identity;
    }
}
