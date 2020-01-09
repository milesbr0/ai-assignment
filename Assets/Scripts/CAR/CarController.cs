using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rigidBody; //rigidbody of the car
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
    }

    private void Move(float vertical, float horizontal)
    {
        rigidBody.velocity = transform.right * vertical * verticalSpeed;
        rigidBody.angularVelocity = transform.up * horizontal * horizontalSpeed;
    }
}
