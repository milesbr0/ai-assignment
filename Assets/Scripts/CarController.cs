using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rigidBody; //rigidbody of the car
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    public LayerMask rayCastMask;

    public NeuralNetwork network;

    private float[] input = new float[5];//input to the neural network

    public bool manualControl;
    public int checkpointPosition;
    public bool isCollided;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (manualControl)
        {
            Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

        }
        else
        {
            if (!isCollided)//if the car has not collided with the wall, it uses the neural network to get an output
            {
                for (int i = 0; i < 5; i++)//draws five debug rays as inputs
                {
                    Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right*10;//calculating angle of raycast
                    RaycastHit hit;
                    Ray Ray = new Ray(transform.position, newVector);
                    Debug.DrawRay(transform.position, newVector, Color.red);
                    if (Physics.Raycast(Ray, out hit, 10, rayCastMask))
                    {
                        input[i] = (10 - hit.distance) / 10;//return distance, 1 being close
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                    }
                    else
                    {
                        input[i] = 0;//if nothing is detected, will return 0 to network
                    }
                }

                float[] output = network.FeedForward(input);//Call to network to feedforward

                transform.Rotate(0, output[0] * rotationSpeed, 0, Space.World);//controls the cars movement
                transform.position += this.transform.right * output[1] * moveSpeed;//controls the cars turning
            }

        }
    }

    private void Move(float vertical, float horizontal)
    {
        rigidBody.velocity = transform.right * vertical * moveSpeed;
        rigidBody.angularVelocity = transform.up * horizontal * rotationSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))//check if the car passes a gate
        {
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (collision.collider.gameObject == checkPoints[i] && i == (checkpointPosition + 1 + checkPoints.Length) % checkPoints.Length)
                {
                    checkpointPosition++;//if the gate is one ahead of it, it increments the position, which is used for the fitness/performance of the network
                    break;
                }
            }
        }
        else if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Learner"))
        {
            isCollided = true;//stop operation if car has collided
        }
    }

    public void UpdateFitness()
    {
        network.fitness = checkpointPosition;//updates fitness of network for sorting
    }
}
