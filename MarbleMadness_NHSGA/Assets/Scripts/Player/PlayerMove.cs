using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //components
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
    public int gravity;
    
    //movement
    private Vector3 moveRawInput;
    private Vector3 moveDirection;
    [SerializeField] float moveForce;
    private bool hasBeenInGoal = false;

    private float yOnExit = 0;
    private int numOfColliders = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = cam.transform.TransformDirection(moveRawInput); //set move direction relative to camera veiw
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); //stop move direction from movign player upward
    }

    void FixedUpdate()
    {
        //rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Acceleration);
        rb.AddForce(moveDirection * moveForce, ForceMode.Impulse);
    }

    void OnMove(InputValue value)
    {
        moveRawInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            // Allow the ball to travel a little into the platform before clearing its velocity.
            if (!hasBeenInGoal)
            {
                Invoke("GoalReached", 1);
                hasBeenInGoal = true;
            }
            else GoalReached();
        }

        else if (collision.gameObject.tag == "ground")
        {
            numOfColliders += 1;
            if (numOfColliders == 1)
            {
                float distance = yOnExit - gameObject.transform.position.y;
                if (distance > 3 && distance < 5) Debug.Log("dizzy");
                else if (distance > 5) Debug.Log("crack");
            }
            yOnExit = gameObject.transform.position.y;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            numOfColliders -= 1;
            yOnExit = gameObject.transform.position.y;
        }
    }

    void GoalReached()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
    }
}
