using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ANNManager : MonoBehaviour
{

    //GUI
    public Text generationText;
    public Text timeLeftText;
    public Text bestFitnessText;
    private int generationNumber = 0;
    private float timeLeft;
    private float bestFitness;
    private int carsLeft;


    [SerializeField] private Material bestCarMaterial;
    [SerializeField] private Material averageCarMaterial;

    public float waweTime;
    public int populationSize;
    public GameObject carPrefab;
    public Transform spawnPoint;
    private Vector3 spawnPointCoord;

    //neural network initialization
    public int[] networkSize = new int[3] { 5, 3, 2 };

    public List<NeuralNetwork> ANNModels;
    private List<CarController> cars;

    void Start()
    {
        InitNetworks();
        InvokeRepeating("SpawnCars", 0.1f, waweTime);

        spawnPointCoord = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
    }

    private void Update()
    {
        generationText.text = "Generation: " + generationNumber;
        timeLeft -= 1 * Time.deltaTime;
        timeLeftText.text = "Time left: " + Mathf.FloorToInt(timeLeft+1);
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].checkpointPosition >= bestFitness)
            {
                bestFitness = cars[i].checkpointPosition;
                if (bestFitness != 0)
                {
                    cars[i].GetComponent<Renderer>().material = bestCarMaterial;
                }
            }
            else
                cars[i].GetComponent<Renderer>().material = averageCarMaterial;
        }

        bestFitnessText.text = "Best fitness: " + bestFitness;

    }

    public void InitNetworks()
    {
        ANNModels = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(networkSize);
            ANNModels.Add(net);
        }
    }

    public void SpawnCars()
    {
        timeLeft = waweTime;
        carsLeft = populationSize;
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject); //delete previous generation
            }
            generationNumber++;
            UpdateANN();//update and mutate next generation's neural network
        }

        cars = new List<CarController>();
        for (int i = 0; i < populationSize; i++)
        {
            CarController car = (Instantiate(carPrefab, spawnPointCoord, new Quaternion(0, 0, 1, 0))).GetComponent<CarController>();//create botes
            car.network = ANNModels[i];
            cars.Add(car);
        }

    }

    public void UpdateANN()
    {
        for (int i = 0; i < populationSize; i++)
            cars[i].UpdateFitness();

        ANNModels.Sort();
        for (int i = 0; i < populationSize / 2; i++)
        {
            ANNModels[i] = ANNModels[i + populationSize / 2].copy(new NeuralNetwork(networkSize));
            ANNModels[i].Mutate((int)(1 / 0.01f), 0.5f);
        }
    }

    private void VisualizeANN()
    {
        //TODO
    }

}