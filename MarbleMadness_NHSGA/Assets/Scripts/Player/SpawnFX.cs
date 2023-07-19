using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFX : MonoBehaviour
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
            player.GetComponent<PlayerMove>().canMove = true;

            Destroy(gameObject);
        }
    }
}
