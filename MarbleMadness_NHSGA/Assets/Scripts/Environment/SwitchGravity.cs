using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    [SerializeField] Vector3 gravityDirectionToSwitchTo;

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
            gravityDirectionToSwitchTo = (col.contacts[0].normal).normalized * -1;
            col.gameObject.GetComponent<PlayerMove>().SetGravity(gravityDirectionToSwitchTo);
        }
    }
}
