using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    //aggro
    [SerializeField] float aggroRadius;
    private Transform playerPosition;

    //tracking
    private Vector3 moveDirection;
    [SerializeField] float moveForce;
    [SerializeField] Rigidbody rb;

    //sound
    [SerializeField] AudioSource collisionSound;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable()
    {
        this.gameObject.GetComponent<BallMovement>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAggro();

        moveDirection = (playerPosition.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        rb.AddForce(moveDirection * moveForce, ForceMode.Impulse);
    }

    void CheckAggro()
    {
        var ballMovement = gameObject.GetComponent<BallMovement>();
        if (Vector3.Distance(transform.position, playerPosition.position) > aggroRadius
            || Vector3.Distance(transform.position, ballMovement.origin) > ballMovement.boundSize
        ) {
            //enable movescript
            this.gameObject.GetComponent<BallMovement>().enabled = true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player")) {
            collisionSound.Play();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
