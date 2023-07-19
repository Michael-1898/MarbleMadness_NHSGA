using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] GameObject checkpoint;
    [SerializeField] GameObject farCheckpoint;
    private int checkpointCounter;
    [SerializeField] Rigidbody rb;
    private GameObject player;

    [SerializeField] AudioSource respawnSound;
    [SerializeField] GameObject deathFX;
    public bool isFar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("ground") && player.GetComponent<PlayerMove>().getDistance() < 3) {
            //occasional checkpoints
            checkpointCounter++;
            if(checkpointCounter > 17) {
                farCheckpoint.transform.position = checkpoint.transform.position;
                checkpointCounter = 0;
            }

            checkpoint.transform.position = new Vector3(col.gameObject.transform.position.x, col.gameObject.transform.position.y + (col.gameObject.transform.localScale.y) + 0.5f, col.gameObject.transform.position.z);
        }
    }

    public void SendToCheckpoint()
    {
        //play death animation and sound or whatever
        respawnSound.Play();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerMove>().canMove = false;
        Instantiate(deathFX, transform.position, Quaternion.identity);
        
        //send back to checkpoint once animation/sound has played
        // transform.position = checkpoint.transform.position;
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;
    }

    public void SendToFarCheckpoint()
    {
        //play death animation and sound or whatever
        respawnSound.Play();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerMove>().canMove = false;
        Instantiate(deathFX, transform.position, Quaternion.identity);
        
        //send back to checkpoint once animation/sound has played
        // transform.position = farCheckpoint.transform.position;
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;
    }

    public Vector3 getCheckpoint()
    {
        return checkpoint.transform.position;
    }

    public Vector3 getFarCheckpoint()
    {
        return farCheckpoint.transform.position;
    }
}
