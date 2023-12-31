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
    public GameObject canvas;

    //sound
    [SerializeField] AudioSource marbleRoll;
    private bool rollSoundPlaying;
    [SerializeField] AudioSource breakSound;
    [SerializeField] AudioSource dizzySound;
    [SerializeField] AudioSource fallSound;
    [SerializeField] AudioSource collisionSound;
    
    //movement
    private Vector3 moveRawInput;
    private Vector3 moveDirection;
    [SerializeField] float moveForce;
    public bool hasBeenInGoal = false;
    [SerializeField] float topSpeed;
    public bool canMove = false;
    [SerializeField] float pooshAmount;
    private bool isDizzy = false;
    private float distance;

    //gravity
    private float gravity = 9.81f;
    [SerializeField] float gravityMultiplier;
    private bool gravityOn = true;
    [SerializeField] GameObject container;
    private Vector3 gravityDir = new Vector3(0, -1, 0);

    //FX
    [SerializeField] GameObject dizzyFX;

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
        // //WASD movement
        // moveDirection = cam.transform.TransformDirection(moveRawInput); //set move direction relative to camera veiw
        // moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); //stop move direction from movign player upward

        //mouse movement
        if(gravityDir == new Vector3(0, -1, 0)) {
            moveRawInput = Input.mousePosition;
            Vector3 ballPosition = cam.WorldToScreenPoint(transform.position);
            moveRawInput.x -= ballPosition.x;
            moveRawInput.y -= ballPosition.y;
            moveDirection = new Vector3(moveRawInput.x, 0, moveRawInput.y); //stop move direction from movign player upward
            moveDirection = cam.transform.TransformDirection(moveDirection);
        } else if(gravityDir == new Vector3(0, 0, 1)) {
            moveRawInput = Input.mousePosition;
            Vector3 ballPosition = cam.WorldToScreenPoint(transform.position);
            moveRawInput.x -= ballPosition.x;
            moveRawInput.y -= ballPosition.y;
            moveDirection = new Vector3(moveRawInput.x, moveRawInput.y, 0); //stop move direction from movign player upward
            moveDirection = cam.transform.TransformDirection(moveDirection);
        } else if(gravityDir == new Vector3(1, 0, 0)) {
            moveRawInput = Input.mousePosition;
            Vector3 ballPosition = cam.WorldToScreenPoint(transform.position);
            moveRawInput.x -= ballPosition.x;
            moveRawInput.y -= ballPosition.y;
            moveDirection = new Vector3(0, moveRawInput.y, -moveRawInput.x); //stop move direction from movign player upward
            moveDirection = cam.transform.TransformDirection(moveDirection);
        } else if(gravityDir == new Vector3(-1, 0, 0)) {
            moveRawInput = Input.mousePosition;
            Vector3 ballPosition = cam.WorldToScreenPoint(transform.position);
            moveRawInput.x -= ballPosition.x;
            moveRawInput.y -= ballPosition.y;
            moveDirection = new Vector3(0, moveRawInput.y, moveRawInput.x); //stop move direction from movign player upward
            moveDirection = cam.transform.TransformDirection(moveDirection); 
        }
        
        
        if(numOfColliders > 0 && canMove) {
            if(!rollSoundPlaying) {
                marbleRoll.Play();
                rollSoundPlaying = true;
            }
            marbleRoll.volume = rb.velocity.magnitude/(topSpeed+1.5f);
        }

        if(!canMove) {
            marbleRoll.Stop();
            rollSoundPlaying = false;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, topSpeed);
    }

    void FixedUpdate()
    {
        //rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Acceleration);
        if(canMove && !isDizzy) {
            rb.AddForce(moveDirection * moveForce, ForceMode.Impulse);
        }

        //gravity
        if(gravityOn) {
            rb.AddForce(gravityDir * gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    // //WASD movement
    // void OnMove(InputValue value)
    // {
    //     moveRawInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            // Allow the ball to travel a little into the platform before clearing its velocity.
            if (!hasBeenInGoal)
            {
                Invoke("GoalReached", 1);
                hasBeenInGoal = true;
                marbleRoll.Stop();
            }
            else GoalReached();
        }

        else if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "ramp")
        {
            numOfColliders += 1;
            if (numOfColliders == 1)
            {
                if(gravityDir == new Vector3(0, -1, 0)) {
                    distance = yOnExit - gameObject.transform.position.y;
                } else if(gravityDir == new Vector3(0, 0, 1)) {
                    distance = yOnExit + gameObject.transform.position.z;
                } else if(gravityDir == new Vector3(1, 0, 0)) {
                    distance = yOnExit + gameObject.transform.position.x;
                } else if(gravityDir == new Vector3(-1, 0, 0)) {
                    distance = yOnExit - gameObject.transform.position.x;
                }
                
                if (distance > 1.4f && distance < 3f) {
                    dizzySound.Play();
                    isDizzy = true;
                    Invoke("Dizzynt", 2);
                    Instantiate(dizzyFX, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                }
                else if (distance > 3f) {
                    var thing = canvas.transform.GetChild(3).gameObject.GetComponent<ScoreManager>();
                    //print("i should probably do this, thing: " + thing.scoreCanStartScoringYeah);
                    thing.UpdateScore(-100);

                    //play break sound
                    breakSound.Play();

                    //play break animation
                    GetComponent<CheckpointManager>().SendToCheckpoint();
                }
                else if(distance > 1f) {
                    fallSound.Play();
                }
            }
            if(gravityDir == new Vector3(0, -1, 0)) {
                yOnExit = gameObject.transform.position.y;
            } else if(gravityDir == new Vector3(0, 0, 1)) {
                yOnExit = gameObject.transform.position.z;
            } else if(gravityDir == new Vector3(1, 0, 0) || gravityDir == new Vector3(-1, 0, 0)) {
                yOnExit = gameObject.transform.position.x;
            } 
        }

        if (collision.gameObject.tag == "ramp")
        {
            gravityOn = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("ramp") && !collision.gameObject.CompareTag("ground") 
        && !collision.gameObject.CompareTag("goal") && !collision.gameObject.CompareTag("DeathZone") && !collision.gameObject.CompareTag("enemy")
        && collision.gameObject.transform.position.y + collision.transform.localScale.y >= transform.position.y) {
            collisionSound.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "ramp")
        {
            numOfColliders -= 1;
            if(gravityDir == new Vector3(0, -1, 0)) {
                yOnExit = gameObject.transform.position.y;
            } else if(gravityDir == new Vector3(0, 0, 1)) {
                yOnExit = gameObject.transform.position.z;
            } else if(gravityDir == new Vector3(1, 0, 0) || gravityDir == new Vector3(-1, 0, 0)) {
                yOnExit = gameObject.transform.position.x;
            } 
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

    // Player Knockback
    private void OnCollisionStay(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Transform transform = collision.gameObject.transform;

        if (
            (!collision.gameObject.CompareTag("ground") && !collision.gameObject.CompareTag("ramp") && new List<Vector3>() { transform.right, -transform.right, transform.forward, -transform.forward }.Contains(normal))
            || collision.gameObject.tag == "enemy"
            )
        {
            rb.AddForce(normal * pooshAmount * (collision.gameObject.tag == "enemy" ? 2 : 1), ForceMode.Impulse);
        }
    }

    //  (\ /)
    //  (o o)
    //  (U U)o

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

        var timerManager = canvas.transform.GetChild(1).gameObject.GetComponent<UITimer>();
        var scoreManager = canvas.transform.GetChild(3).gameObject.GetComponent<ScoreManager>();
        scoreManager.UpdateScore(5000);
        scoreManager.UpdateScore(100 * Mathf.FloorToInt(timerManager.GetTimer()));
    }

    void Dizzynt()
    {
        isDizzy = false;
    }

    public float getDistance()
    {
        return distance;
    }

    public void SetGravity(Vector3 direction)
    {
        gravityDir = direction;
    }

    public Vector3 GetGravity()
    {
        return gravityDir;
    }
}
