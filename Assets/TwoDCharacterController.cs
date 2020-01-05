using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCharacterController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 0.35f;
    [SerializeField] private Vector2 jumpHeight;
    private Rigidbody2D rigidbody2D;
    private bool isJumping = false;
    private bool isCrouching = false;
    private Camera camera;



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

        if(Input.GetKey(KeyCode.D))
        {
            vertical += moveSpeed ;
        }

        if (Input.GetKey(KeyCode.A))
        {
            vertical -= moveSpeed;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            isJumping = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y * 0.5f);
            isCrouching = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y * 2.0f);
            isCrouching = false;
        }


        gameObject.transform.position = new Vector2(vertical, horizontal);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground") isJumping = false;
    }

}
