using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public Transform head;
    public float speed = 4f;
    public float playerAcceleration = 2f;

    [HideInInspector] public Rigidbody rb;
    private Vector3 direction;

    public bool movementLocked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!movementLocked)
        {
            direction = Input.GetAxisRaw("Horizontal") * head.right + Input.GetAxisRaw("Vertical") * head.forward;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction.normalized * speed + rb.linearVelocity.y * Vector3.up, playerAcceleration * Time.deltaTime);
        }
        else
            return;
    }
}