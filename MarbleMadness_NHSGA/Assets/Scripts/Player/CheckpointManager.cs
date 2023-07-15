using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] GameObject checkpoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("ground")) {
            checkpoint.transform.position = new Vector3(col.gameObject.transform.position.x, col.gameObject.transform.position.y + 0.7f, col.gameObject.transform.position.z);
        }
    }
}
