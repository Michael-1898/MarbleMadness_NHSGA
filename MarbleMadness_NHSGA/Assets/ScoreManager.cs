using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    GameObject player;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpeedScore", 0, 1);
        player = gameObject.GetComponent<PlayerMove>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpeedScore()
    {
        
    }
}
