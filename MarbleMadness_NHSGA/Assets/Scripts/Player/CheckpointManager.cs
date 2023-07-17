using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] GameObject checkpoint;
    [SerializeField] Rigidbody rb;

    [SerializeField] AudioSource respawnSound;

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
            checkpoint.transform.position = new Vector3(col.gameObject.transform.position.x, col.gameObject.transform.position.y + (col.gameObject.transform.localScale.y / 1.5f), col.gameObject.transform.position.z);
        }
    }

    public void SendToCheckpoint()
    {
        //play death animation and sound or whatever
        respawnSound.Play();
        
        //send back to checkpoint once animation/sound has played
        transform.position = checkpoint.transform.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
