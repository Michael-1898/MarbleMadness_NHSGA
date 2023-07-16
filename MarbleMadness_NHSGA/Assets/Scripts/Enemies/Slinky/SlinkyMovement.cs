using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyMovement : MonoBehaviour
{
    //movement
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject Slinky;
    [SerializeField] private float speed;

    //misc
    private Rigidbody rb;

    //aggro
    [SerializeField] private Transform playerTransform;
    [SerializeField] float aggroRadius;

    //targeting
    private Vector3 currentTarget;
    private int currentTargetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = Slinky.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Slinky.GetComponent<SlinkyAttack>().enabled = false;
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is in aggro range go to attack behavior
        if(Vector3.Distance(rb.position, playerTransform.position) < aggroRadius) {
            Slinky.GetComponent<SlinkyAttack>().enabled = true;
        }

        //if reached current target, set next target
        if (Vector3.Distance(rb.position, currentTarget) < 1)
        {
            SetNextTarget();
        }
    }

    private void FixedUpdate()
    {
        //move towards current target position
        rb.MovePosition(Vector3.MoveTowards(rb.position, currentTarget, speed * Time.fixedDeltaTime));
        print(currentTargetIndex);
        print(currentTarget);
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
        Gizmos.DrawWireSphere(Slinky.transform.position, aggroRadius);
    }
}
