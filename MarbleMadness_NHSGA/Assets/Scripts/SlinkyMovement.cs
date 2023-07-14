using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject Slinky;
    [SerializeField] private float speed;

    private Rigidbody rb;

    private Vector3 currentTarget;
    private int currentTargetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetNextTarget();
        rb = Slinky.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, currentTarget, speed * Time.fixedDeltaTime));
        if (Vector3.Distance(rb.position, currentTarget) < 0.5f)
        {
            SetNextTarget();
        }
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
}
