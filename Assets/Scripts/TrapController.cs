using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] bool shouldRotate;
    private Transform player;
    private Transform respawnPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint").GetComponent<Transform>();
    }

    void Update()
    {
        if (shouldRotate)
            Rotate();
    }

    private void Rotate()
    {
        float rotationZ = gameObject.transform.rotation.z;
        rotationZ += rotationSpeed * Time.deltaTime;
        gameObject.transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, rotationZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = respawnPoint.transform.position;

    }

}
