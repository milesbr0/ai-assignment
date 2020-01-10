using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public float timeframe;
    public int populationSize;//creates population size
    public GameObject prefab;//holds bot prefab
    public Transform spawnPoint;
    private Vector3 spawnPointCoord;

    public int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f;

    [Range(0f, 1f)] public float MutationStrength = 0.5f;

    [Range(0.1f, 10f)] public float Gamespeed = 1f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<CarController> cars;

    void Start()// Start is called before the first frame update
    {
        if (populationSize % 2 != 0)
            populationSize = 50;//if population size is not even, sets it to fifty

        InitNetworks();
        InvokeRepeating("CreateBots", 0.1f, timeframe);//repeating function

        spawnPointCoord = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
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
        Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }

            UpdateANN();//this sorts networks and mutates them
        }

        cars = new List<CarController>();
        for (int i = 0; i < populationSize; i++)
        {
            CarController car = (Instantiate(prefab, spawnPointCoord, new Quaternion(0, 0, 1, 0))).GetComponent<CarController>();//create botes
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
            networks[i].Mutate((int)(1 / MutationChance), MutationStrength);
        }
    }
}