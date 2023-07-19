using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterFX : MonoBehaviour
{
    private float FXTimer;
    [SerializeField] float fxDuration;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FXTimer += Time.deltaTime;
        if(FXTimer >= fxDuration) {
            RespawnPlayer();

            player.GetComponent<MeshRenderer>().enabled = true;
            player.GetComponent<PlayerMove>().canMove = true;

            Destroy(gameObject);
        }
    }

    void RespawnPlayer()
    {
        if(player.GetComponent<CheckpointManager>().isFar) {
            player.transform.position = player.GetComponent<CheckpointManager>().getFarCheckpoint();
        } else {
            player.transform.position = player.GetComponent<CheckpointManager>().getCheckpoint();
        }
    }
}
