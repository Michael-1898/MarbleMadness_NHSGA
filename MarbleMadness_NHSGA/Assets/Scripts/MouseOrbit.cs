using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public GameObject target;
    private float xRot = 0f;
    private float yRot = 0f;
    private float distance = 5f;
    public float sensitivity = 50f;
    private Vector3 point;
    //private float minFov = 15f;
    //private float maxFov = 90f;

    void Start()
    {
        point = target.transform.position;
        transform.LookAt(point);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            xRot += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            yRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

            if (xRot > -10f)
            {
                xRot = -10f;
            }
            else if (xRot < -45f)
            {
                xRot = -45f;
            }
            transform.position = target.transform.position + Quaternion.Euler(xRot, yRot, 0f) * (distance * -Vector3.back);
            transform.LookAt(target.transform.position, Vector3.up);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 1)
            {
                Camera.main.fieldOfView--;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView < 1000)
            {
                Camera.main.fieldOfView++;
            }
        }
    }
}