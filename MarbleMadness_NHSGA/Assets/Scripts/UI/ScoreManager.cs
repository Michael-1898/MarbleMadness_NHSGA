using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    GameObject player;
    public GameObject timer;
    public GameObject scoreTxt;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("SpeedScore", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.GetComponent<TMP_Text>().text = "<mspace=21pxem>Score: " + score;
    }

    void SpeedScore()
    {
        float magnitude = player.GetComponent<Rigidbody>().velocity.magnitude;
        UpdateScore(Mathf.FloorToInt(magnitude * 10));
    }

    void UpdateScore(int added)
    {
        var uiTimer = timer.GetComponent<UITimer>();
        if (!uiTimer.gameIsOver && uiTimer.playerReady)
        {
            score += added;
        }
    }
}
