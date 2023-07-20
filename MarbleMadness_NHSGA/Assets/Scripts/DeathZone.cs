using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public GameObject canvas;
    private GameObject player;

    [SerializeField] AudioSource marbleBreak;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player") && player.GetComponent<PlayerMove>().canMove) {
            player.GetComponent<CheckpointManager>().SendToCheckpoint();
            marbleBreak.Play();
        }

        var scoreManager = canvas.transform.GetChild(3).gameObject.GetComponent<ScoreManager>();
        if (col.gameObject.CompareTag("enemy")) {
            switch (col.gameObject.GetComponent<EnemyType>().GetType())
            {
                case "slinky":
                    scoreManager.UpdateScore(2000);
                    break;
                case "marble":
                    scoreManager.UpdateScore(1000);
                    break;
                default:
                    break;
            }

            Destroy(col.gameObject);
        }
    }
}
