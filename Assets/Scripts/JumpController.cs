using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour {

    [SerializeField] private GameObject Platform;

    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private float _startTime;
    private int jumpCount = 0;
    private GameObject _currentPlatform;

    [SerializeField] private float jumpForce = 7.5f;

    void Start () {
        _currentPlatform = Platform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Vector3.zero;
        _direction = new Vector3(0, 0, 1);
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {

            _startTime = Time.time;
            OnTappingStart();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {

            var elapse = Time.time - _startTime;
            Jump(elapse);
            OnTappingEnd();
            jumpCount += 1;
        }
    }

    void Jump(float elapse) {

        Debug.Log(elapse);
        _rigidbody.AddForce((_direction + new Vector3(0, 1, 0)) * elapse * jumpForce, ForceMode.Impulse);
    }

    private void OnTappingStart() {
       
        //TODO
    }

    private void OnTappingEnd() {
        
        //TODO
    }
}
