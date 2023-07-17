using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    //text objects
    [SerializeField] GameObject timerTxt;
    [SerializeField] GameObject timeDisclaimer;

    //timing and display
    private float timer;
    [SerializeField] float disclaimerDuration;
    private bool disclaimerDone = false;
    private bool timerFull = false;
    [SerializeField] float levelTime;

    //player enabling
    private GameObject player;
    private Vector3 playerOrigin;
    private bool playerHasMoved = false;
    private float startTimer;
    private bool playerReady = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        EnablePlayer();

        if(playerReady) {
            CheckIfPlayerMoved();
        }

        DisplayDisclaimer();

        if(disclaimerDone && !timerFull) {
            FillUpTimer();
        }

        if(timerFull && playerHasMoved) {
            timeDisclaimer.SetActive(false);

            DecreaseTimer();
        }
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    void DisplayDisclaimer()
    {
        if(disclaimerDuration > 0) {
            disclaimerDuration -= Time.deltaTime;
        }
        
        if(disclaimerDuration <= 0 && !disclaimerDone) {
            disclaimerDone = true;
        }
    }

    void FillUpTimer()
    {
        //increase timer
        timer += 25 * Time.deltaTime;
        timerTxt.GetComponent<TMP_Text>().text = "<mspace=21pxem>" + (Mathf.Round(timer * 100f) / 100f).ToString("f2");
        
        if(timer >= 60) {
            timerTxt.GetComponent<TMP_Text>().text = "60";
            timerFull = true;
        }

        //decrease disclaimer time
        if(levelTime - timer < 0) {
            timeDisclaimer.GetComponent<TMP_Text>().text = "<mspace=23pxem>Time to finish race: 0";
        } else {
            timeDisclaimer.GetComponent<TMP_Text>().text = "<mspace=23pxem>Time to finish race: " + (Mathf.Round((levelTime - timer) * 100f) / 100f).ToString("f2");
        }
    }

    void DecreaseTimer()
    {
        timer -= Time.deltaTime;
        timerTxt.GetComponent<TMP_Text>().text = "<mspace=21pxem>" + (Mathf.Round(timer * 100f) / 100f).ToString("f2");
    }

    void CheckIfPlayerMoved()
    {
        if(player.transform.position != playerOrigin && !playerHasMoved) {
            playerHasMoved = true;
        }
    }

    void EnablePlayer()
    {
        startTimer += Time.deltaTime;
        if(startTimer > 2.5f && !playerReady) {
            playerReady = true;
            playerOrigin = player.transform.position;
        }
    }
}
