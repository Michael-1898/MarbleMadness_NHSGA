using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] GameObject checkpoint;
    [SerializeField] GameObject farCheckpoint;
    private int checkpointCounter;
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
            //occasional checkpoints
            checkpointCounter++;
            if(checkpointCounter > 12) {
                farCheckpoint.transform.position = checkpoint.transform.position;
                checkpointCounter = 0;
            }

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

    public void SendToFarCheckpoint()
    {
        //play death animation and sound or whatever
        respawnSound.Play();
        
        //send back to checkpoint once animation/sound has played
        transform.position = farCheckpoint.transform.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
