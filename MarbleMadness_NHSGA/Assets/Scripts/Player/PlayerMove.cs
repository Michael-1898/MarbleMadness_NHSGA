using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //components
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
    
    //movement
    private Vector3 moveRawInput;
    private Vector3 moveDirection;
    [SerializeField] float moveForce;
    private bool hasBeenInGoal = false;

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
            // Allow the ball to go to travel a little into the platform before clearing its velocity.
            if (!hasBeenInGoal)
            {
                Invoke("GoalReached", 1);
                hasBeenInGoal = true;
            }
            else GoalReached();
        }
    }

    void GoalReached()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
    }
}
