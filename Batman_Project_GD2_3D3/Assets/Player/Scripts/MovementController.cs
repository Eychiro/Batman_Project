using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform head;
    public float speed = 4f;
    public float playerAcceleration = 2f;

    private Rigidbody rb;
    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal") * head.right + Input.GetAxisRaw("Vertical") * head.forward;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction.normalized * speed + rb.linearVelocity.y * Vector3.up, playerAcceleration * Time.deltaTime);
    }
}
