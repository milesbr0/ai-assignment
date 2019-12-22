using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour{

    private Transform player;
    private Transform respawnPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other){
        player.transform.position = respawnPoint.transform.position;
    }
}
