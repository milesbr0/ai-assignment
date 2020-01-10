using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpController : MonoBehaviour {

    [SerializeField] private GameObject Platform;

    private Vector3 jumpDirection;
    private Rigidbody rigidBody;
    private float jumpStartTime;
    private int jumpCount = 0;
    private GameObject currentPlatform;

    [SerializeField] private float jumpForce = 7.5f;

    void Start () {
        currentPlatform = Platform;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = Vector3.zero;
        jumpDirection = new Vector3(0, 0, 1);
        SpawnNextStage();
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {

            jumpStartTime = Time.time;
            OnTappingStart();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {

            var elapse = Time.time - jumpStartTime;
            Jump(elapse);
            OnTappingEnd();
            jumpCount += 1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentPlatform.transform.localScale -= new Vector3(0, 0.15f, 0) * Time.deltaTime;
            currentPlatform.transform.position -= new Vector3(0, 0.15f, 0) * Time.deltaTime;
        }
    }

    void Jump(float elapse) {

        Debug.Log(elapse);
        rigidBody.AddForce((jumpDirection + new Vector3(0, 1, 0)) * elapse * jumpForce, ForceMode.Impulse);
    }

    private void OnTappingStart() {
       
        //TODO
    }

    private void OnTappingEnd() {
        currentPlatform.transform.DOScaleY(0.5f, 0.2f);
        currentPlatform.transform.DOMoveY(-0.25f, 0.2f);
        SpawnNextStage();
    }

    private void SpawnNextStage()
    {
        var nextStage = Instantiate(Platform);

        nextStage.transform.position = currentPlatform.transform.position + jumpDirection * Random.Range(1.9f, 3.5f);

        //random scale
        var originalScale = Platform.transform.localScale;
        var scaleFactor = Random.Range(0.25f, 1.5f);
        var newScale = originalScale * scaleFactor;
        newScale.y = originalScale.y;
        nextStage.transform.localScale = newScale;

        //random color
        nextStage.GetComponent<Renderer>().material.color =
            new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        currentPlatform = nextStage;

    }
}
