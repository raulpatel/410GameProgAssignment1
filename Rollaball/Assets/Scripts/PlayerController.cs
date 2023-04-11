using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jump = 300;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    private Rigidbody rb;
    private int count;
    private bool jumpKeyDown;
    private bool canDoubleJump;
    private bool grounded;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        count = 0;
        canDoubleJump = false;
        grounded = true;
        
        SetCountText();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
    }

    void OnMove(InputValue movementValue) 
    {
        jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        movementX = movementVector.x;
        movementY = movementVector.y;
        Jump(jumpKeyDown);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    void Jump(bool jumpKeyDown) {
        // double jump logic from https://answers.unity.com/questions/643228/double-jump-2.html
        if (jumpKeyDown) 
        {
            Debug.Log("Jump pressed.");
            if (grounded) 
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);//, ForceMode.VelocityChange);
                rb.AddForce(new Vector3(0, jump, 0));
                canDoubleJump = true;
            } 
            else if (canDoubleJump) 
            {
                canDoubleJump = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);//, ForceMode.VelocityChange);
                rb.AddForce(new Vector3(0, jump, 0));
            }
        }
    }

    void FixedUpdate()
    {
        if (rb.position.y < 0) 
        {
            loseTextObject.SetActive(true);
        }
        else if (rb.position.y > 0.5)
        {
            grounded = false;
        }
        else 
        {
            grounded = true;
        }
        jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        Jump(jumpKeyDown);
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        rb.AddForce(movement * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        Jump(jumpKeyDown);
    }
}
