using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyAttack : MonoBehaviour
{
    //misc
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject SlinkyMover;
    [SerializeField] Animator anim;

    //sound
    [SerializeField] AudioSource marbleCollision;

    //aggro
    [SerializeField] float aggroRadius;

    //attack
    [SerializeField] float attkCooldown;
    private float attkTimer;
    [SerializeField] float jumpForce;
    private GameObject player;
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
        player = GameObject.FindWithTag("Player");
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
            anim.SetBool("isFalling", false);
        }
        
        if(attkTimer >= attkCooldown && !isJumping && !player.GetComponent<PlayerMove>().hasBeenInGoal) {
            Jump();
            attkTimer = 0;
            groundedTimer = groundedCooldown;
        }

        //if player is outside aggro range return to movement script
        var slinkyMovement = SlinkyMover.GetComponent<SlinkyMovement>();
        if((Vector3.Distance(rb.position, playerTransform.position) > aggroRadius && !isJumping)
            || (Vector3.Distance(rb.position, slinkyMovement.origin) > slinkyMovement.boundSize && !isJumping || !player.GetComponent<PlayerMove>().canMove)
        ) {
            SlinkyMover.GetComponent<SlinkyMovement>().enabled = true;
        }

        if(isJumping && rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
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
        anim.SetBool("isJumping", true);

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

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player") && !isGrounded) {
            marbleCollision.Play();

            player.GetComponent<CheckpointManager>().SendToFarCheckpoint();
            SlinkyMover.GetComponent<SlinkyMovement>().enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
