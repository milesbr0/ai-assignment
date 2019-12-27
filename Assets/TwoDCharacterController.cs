using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCharacterController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    Rigidbody2D rigidbody2D;


    bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = gameObject.transform.position.x;
        float horizontal = gameObject.transform.position.y;

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            vertical += moveSpeed ;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            vertical -= moveSpeed;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) && isJumping == false)
        {
            rigidbody2D.AddForce(new Vector2(0, jumpHeight),ForceMode2D.Impulse);
           // isJumping = true;
        }



        gameObject.transform.position = new Vector2(vertical, horizontal);
    }
}
