using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyAttack : MonoBehaviour
{
    //misc
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject SlinkyMover;

    //sound
    [SerializeField] AudioSource marbleCollision;

    //aggro
    [SerializeField] float aggroRadius;

    //attack
    [SerializeField] float attkCooldown;
    private float attkTimer;
    [SerializeField] float jumpForce;
    private Transform playerTransform;
    private Vector3 attkDirection;
    [SerializeField] float lateralAttkSpeed;
    private Vector3 lateralDirection;
    private bool isJumping = false;

    //ground check (for attack)
    private bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;
    private float groundedTimer;
    private float groundedCooldown = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable()
    {
        SlinkyMover.GetComponent<SlinkyMovement>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        if(!isJumping) {
            attkTimer += Time.deltaTime;
        }

        if(isJumping && isGrounded) {
            isJumping = false;
        }
        
        if(attkTimer >= attkCooldown && !isJumping) {
            Jump();
            attkTimer = 0;
            groundedTimer = groundedCooldown;
        }

        //if player is outside aggro range return to movement script
        if(Vector3.Distance(rb.position, playerTransform.position) > aggroRadius && !isJumping) {
            SlinkyMover.GetComponent<SlinkyMovement>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        if(isJumping) {
            TrackLaterally();
        }
    }

    private void Jump()
    {   
        isJumping = true;

        //jump in air while doing it
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void TrackLaterally()
    {
        //laterally move towards player
        lateralDirection = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector3(lateralDirection.x * lateralAttkSpeed * Time.deltaTime, rb.velocity.y, lateralDirection.z * lateralAttkSpeed * Time.deltaTime);
    }

    private void GroundCheck()
    {
        if(groundedTimer <= 0) {
            isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
        } else {
            isGrounded = false;
        }

        //starts timer so it doesn't detect ground again while jumping
        if(groundedTimer > 0) {
            groundedTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player")) {
            marbleCollision.Play();
        }
        //send player to checkpoint
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
