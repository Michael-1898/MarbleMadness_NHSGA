using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] Transform end;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player")) {
            col.gameObject.transform.position = end.transform.position;
            col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
