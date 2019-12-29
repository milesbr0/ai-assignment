using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float verticalInput, horizontalInput;

    bool isCrouching;
    float original;

    void Start(){

        controller = GetComponent<CharacterController>();
        original = controller.height;
    }

    void Update() {

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        Vector3 forwardDirection = new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z);
        Vector3 sideDirection = new Vector3(gameObject.transform.right.x, 0, gameObject.transform.right.z);
        forwardDirection.Normalize();
        sideDirection.Normalize();
        forwardDirection = forwardDirection * verticalInput;
        sideDirection = sideDirection * horizontalInput;
        Vector3 finalDirection = forwardDirection + sideDirection;

        if (finalDirection.sqrMagnitude > 1) finalDirection.Normalize();

        if (controller.isGrounded) {
            moveDirection = new Vector3(finalDirection.x, 0, finalDirection.z);
            moveDirection *= 6.0f;
            if (Input.GetButton("Jump"))
                moveDirection.y = 8.0f;

        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouching == false)
        {
            isCrouching = true;
            controller.height = original * 0.35f;

        }

        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            controller.height = original;
        }

        moveDirection.y -= 20.0f * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
