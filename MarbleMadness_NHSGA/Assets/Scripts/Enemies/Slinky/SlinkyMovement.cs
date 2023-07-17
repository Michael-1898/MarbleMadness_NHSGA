using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyMovement : MonoBehaviour
{
    //movement
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject slinky;
    [SerializeField] private float speed;

    //misc
    private Rigidbody rb;

    //aggro
    private Transform playerTransform;
    [SerializeField] float aggroRadius;

    //targeting
    private Vector3 currentTarget;
    private int currentTargetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = slinky.GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable()
    {
        slinky.GetComponent<SlinkyAttack>().enabled = false;
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is in aggro range go to attack behavior
        if(Vector3.Distance(rb.position, playerTransform.position) < aggroRadius) {
            slinky.GetComponent<SlinkyAttack>().enabled = true;
        }

        //if reached current target, set next target
        currentTarget = new Vector3(currentTarget.x, slinky.transform.position.y, currentTarget.z);
        if (Vector3.Distance(slinky.transform.position, currentTarget) < 0.5f)
        {
            SetNextTarget();
        }
    }

    private void FixedUpdate()
    {
        //move towards current target position
        rb.MovePosition(Vector3.MoveTowards(slinky.transform.position, currentTarget, speed * Time.fixedDeltaTime));
    }

    private void SetNextTarget()
    {
        currentTargetIndex++;
        if (currentTargetIndex >= points.Length)
        {
            currentTargetIndex = 0;
        }
        currentTarget = points[currentTargetIndex].position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(slinky.transform.position, aggroRadius);
    }
}
