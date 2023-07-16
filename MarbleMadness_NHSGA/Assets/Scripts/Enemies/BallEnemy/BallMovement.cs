using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    //movement
    [SerializeField] float moveForce;
    [SerializeField] Transform centerOfMovement;
    private Vector3 directionTowardsCenter;

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
        directionTowardsCenter = (centerOfMovement.position - transform.position).normalized;
        print(directionTowardsCenter);
        rb.AddForce(directionTowardsCenter * moveForce, ForceMode.Impulse);

        
    }

    void FindPerpendicular()
    {
        
    }
}
