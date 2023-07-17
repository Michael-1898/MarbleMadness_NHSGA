using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //components
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;

    //sound
    [SerializeField] AudioSource marbleRoll;
    private bool rollSoundPlaying;
    [SerializeField] AudioSource breakSound;
    [SerializeField] AudioSource dizzySound;
    [SerializeField] AudioSource fallSound;
    
    //movement
    private Vector3 moveRawInput;
    private Vector3 moveDirection;
    [SerializeField] float moveForce;
    public bool hasBeenInGoal = false;
    [SerializeField] float topSpeed;

    //gravity
    private float gravity = 9.81f;
    [SerializeField] float gravityMultiplier;
    private bool gravityOn = true;
    [SerializeField] GameObject container;

    //misc
    private float yOnExit = 0;
    private int numOfColliders = 0;
    [SerializeField] GameObject winDisplay;

    // Start is called before the first frame update
    void Start()
    {
        winDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = cam.transform.TransformDirection(moveRawInput); //set move direction relative to camera veiw
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); //stop move direction from movign player upward
        

        if(rb.velocity.magnitude > 0.4f && numOfColliders > 0 && !rollSoundPlaying) {
            marbleRoll.Play();
            rollSoundPlaying = true;
        }

        if((rb.velocity.magnitude < 0.4f || numOfColliders < 1) && rollSoundPlaying) {
            marbleRoll.Stop();
            rollSoundPlaying = false;
        }

        // if(rb.velocity.magnitude > topSpeed) {
        //     rb.velocity.magnitude = topSpeed;
        // }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, topSpeed);
    }

    void FixedUpdate()
    {
        //rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Acceleration);
        rb.AddForce(moveDirection * moveForce, ForceMode.Impulse);

        //gravity
        if(gravityOn) {
            rb.AddForce(-container.transform.up * gravity * gravityMultiplier, ForceMode.Acceleration);
        }
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

        else if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "ramp")
        {
            numOfColliders += 1;
            if (numOfColliders == 1)
            {
                float distance = yOnExit - gameObject.transform.position.y;
                if (distance > 3 && distance < 5) {
                    Debug.Log("dizzy");
                    dizzySound.Play();
                }
                else if (distance > 5) {
                    Debug.Log("crack");

                    //play break sound
                    breakSound.Play();

                    //play break animation
                    GetComponent<CheckpointManager>().SendToCheckpoint();
                }
                else if(distance > 1f) {
                    fallSound.Play();
                }
            }
            yOnExit = gameObject.transform.position.y;
        }

        if (collision.gameObject.tag == "ramp")
        {
            gravityOn = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public int pooshAmount;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "ramp")
        {
            numOfColliders -= 1;
            yOnExit = gameObject.transform.position.y;
        }
        if (collision.gameObject.tag == "ramp")
        {
            gravityOn = true;
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.tag == "ramp")
    //    {
    //        Vector3 rampDir = collision.contacts[0].normal * -1;
    //        rb.AddForce(rampDir.normalized * gravity * normalForce * rb.velocity.magnitude);
    //        Debug.DrawRay(collision.contacts[0].point, -collision.contacts[0].normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);

    //    }
    //    else if (collision.gameObject.tag == "ramp" && -(collision.contacts[0].normal) != new Vector3(0f, -0.71f, 0.71f))
    //    {
    //        Debug.Log("collision is actually " + collision.contacts[0].normal * -1);
    //        Debug.Log("difference = " + (new Vector3(0f, -0.71f, 0.71f) - -collision.contacts[0].normal));
    //    }
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Vector3 normal = collision.contacts[0].normal;
            Transform transform = collision.gameObject.transform;
            if (normal == transform.forward) rb.AddForce(transform.forward * pooshAmount, ForceMode.Impulse);
            else if (normal == -transform.forward) rb.AddForce(-transform.forward * pooshAmount, ForceMode.Impulse);
            else if (normal == transform.right) rb.AddForce(transform.right * pooshAmount, ForceMode.Impulse);
            else if (normal == -transform.right) rb.AddForce(-transform.right * pooshAmount, ForceMode.Impulse);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ramp")
        {
            rb.AddForce(-other.transform.up * gravity * rb.velocity.magnitude);
        }
    }

    void GoalReached()
    {
        rb.velocity = Vector3.zero;
        moveForce = 0;

        winDisplay.SetActive(true);
    }
}
