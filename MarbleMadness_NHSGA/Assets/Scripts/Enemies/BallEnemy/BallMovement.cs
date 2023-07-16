using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float gizmoRadius;

    //movement
    [SerializeField] float xForce;
    [SerializeField] float zForce;
    [SerializeField] Transform centerOfMovement;
    private Vector3 moveDirection;
    [SerializeField] float xBound;
    [SerializeField] float zBound;

    //aggro
    [SerializeField] float aggroRadius;
    private Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();

        moveDirection = new Vector3(xForce, 0, zForce);

        rb.AddForce(moveDirection, ForceMode.Impulse);
    }

    void CheckBounds()
    {
        if(transform.position.x >= centerOfMovement.position.x + xBound) {
            //set x dir neg
            if(xForce > 0) {
                xForce *= -1;
            }
        }
        if(transform.position.x <= centerOfMovement.position.x - xBound) {
            //set x dir pos
            if(xForce < 0) {
                xForce *= -1;
            }
        }

        if(transform.position.z >= centerOfMovement.position.z + zBound) {
            //set z dir neg
            if(zForce > 0) {
                zForce *= -1;
            }
        }
        if(transform.position.z <= centerOfMovement.position.z - zBound) {
            //set z dir pos
            if(zForce < 0) {
                zForce *= -1;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(new Vector3(centerOfMovement.position.x + xBound, centerOfMovement.position.y, centerOfMovement.position.z), gizmoRadius);
        Gizmos.DrawWireSphere(new Vector3(centerOfMovement.position.x - xBound, centerOfMovement.position.y, centerOfMovement.position.z), gizmoRadius);
        Gizmos.DrawWireSphere(new Vector3(centerOfMovement.position.x, centerOfMovement.position.y, centerOfMovement.position.z + zBound), gizmoRadius);
        Gizmos.DrawWireSphere(new Vector3(centerOfMovement.position.x, centerOfMovement.position.y, centerOfMovement.position.z - zBound), gizmoRadius);
    }
}
