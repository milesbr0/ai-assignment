using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{

    //GUI
    public Text generationText;
    public Text timeLeftText;
    public Text bestFitnessText;
    public Text carsLeftText;
    private int generationNumber = 0;
    private float timeLeft;
    private float bestFitness;
    private int carsLeft;

    public float waweTime;
    public int populationSize;//creates population size
    public GameObject carPrefab;//holds bot prefab
    public Transform spawnPoint;
    private Vector3 spawnPointCoord;

    public int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<CarController> cars;

    void Start()// Start is called before the first frame update
    {
        if (populationSize % 2 != 0)
            populationSize = 50;//if population size is not even, sets it to fifty
        InitNetworks();
        InvokeRepeating("CreateBots", 0.1f, waweTime);//repeating function

        spawnPointCoord = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
    }

    private void Update()
    {
        generationText.text = "Generation: " + generationNumber;
        timeLeft -= 1 * Time.deltaTime;
        timeLeftText.text = "Time left: " + Mathf.FloorToInt(timeLeft+1);
        for(int i = 0; i < populationSize; i++)
        {
            if (cars[i].network.fitness > bestFitness) bestFitness = cars[i].network.fitness;
            if (cars[i].isCollided) carsLeft--;
        }
        bestFitnessText.text = "Best fitness: " + bestFitness;
        carsLeftText.text = "Children left: " + carsLeft;

    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            //net.Load("Assets/Save.txt");//on start load the network save
            networks.Add(net);
        }
    }

    public void CreateBots()
    {
        timeLeft = waweTime;
        carsLeft = populationSize;
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }
            generationNumber++;
            UpdateANN();//this sorts networks and mutates them
        }

        cars = new List<CarController>();
        for (int i = 0; i < populationSize; i++)
        {
            CarController car = (Instantiate(carPrefab, spawnPointCoord, new Quaternion(0, 0, 1, 0))).GetComponent<CarController>();//create botes
            car.network = networks[i];//deploys network to each learner
            cars.Add(car);
        }

    }

    public void UpdateANN()
    {
        for (int i = 0; i < populationSize; i++)
            cars[i].UpdateFitness();

        networks.Sort();
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1 / 0.01f), 0.5f);
        }
    }

}