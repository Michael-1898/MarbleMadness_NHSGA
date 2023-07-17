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
    [SerializeField] float width;

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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        EnablePlayer();

        if(player.GetComponent<PlayerMove>().enabled == true) {
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
        timerTxt.GetComponent<TMP_Text>().text = "" + Mathf.Round(timer * 100f) / 100f;
        //timerTxt.SetText($"<mspace={width}em{Mathf.Round(timer * 100f) / 100f}");
        if(timer >= 60) {
            timerTxt.GetComponent<TMP_Text>().text = "60";
            timerFull = true;
        }

        //decrease disclaimer time
        if(levelTime - timer < 0) {
            timeDisclaimer.GetComponent<TMP_Text>().text = "Time to finish race: 0";
        } else {
            timeDisclaimer.GetComponent<TMP_Text>().text = "Time to finish race: " + Mathf.Round((levelTime - timer) * 100f) / 100f;
        }
    }

    void DecreaseTimer()
    {
        timer -= Time.deltaTime;
        timerTxt.GetComponent<TMP_Text>().text = "" + Mathf.Round(timer * 100f) / 100f;
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
        if(startTimer > 2f && player.GetComponent<PlayerMove>().enabled == false) {
            player.GetComponent<PlayerMove>().enabled = true;
            playerOrigin = player.transform.position;
        }
    }
}
