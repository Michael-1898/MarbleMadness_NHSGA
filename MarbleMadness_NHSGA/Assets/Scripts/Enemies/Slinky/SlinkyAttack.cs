using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyAttack : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject SlinkyMover;

    //aggro
    [SerializeField] float aggroRadius;

    //attack
    [SerializeField] float attkCooldown;
    private float attkTimer;
    [SerializeField] float jumpForce;
    [SerializeField] Transform playerTransform;
    private Vector3 attkDirection;
    [SerializeField] float lateralAttkSpeed;
    private Vector3 lateralDirection;
    private bool isJumping = false;

    //ground check (for attack)
    private bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
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
        
        if(attkTimer >= attkCooldown) {
            Jump();
            attkTimer = 0;
        }

        if(isJumping && isGrounded) {
            isJumping = false;
        }

        //if player is outside aggro range return to movement script
        if(Vector3.Distance(rb.position, playerTransform.position) > aggroRadius) {
            SlinkyMover.GetComponent<SlinkyMovement>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        if(isJumping) {
            TrackLaterally();
            print("moving");
        }
    }

    private void Jump()
    {   
        isJumping = true;

        //jump in air while doing it
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        print("jumped");
    }

    private void TrackLaterally()
    {
        //laterally move towards player
        lateralDirection = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector3(lateralDirection.x * lateralAttkSpeed * Time.deltaTime, rb.velocity.y, lateralDirection.y * lateralAttkSpeed * Time.deltaTime);
        
        if(!isJumping) {
            isJumping = true;
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
