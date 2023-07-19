using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private GameObject player;

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
        if(col.gameObject.CompareTag("Player") && player.GetComponent<PlayerMove>().canMove) {
            player.GetComponent<CheckpointManager>().SendToCheckpoint();
        }

        if(col.gameObject.CompareTag("enemy")) {
            Destroy(col.gameObject);
        }
    }
}
