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
    [SerializeField] GameObject disclaimerBG;
    [SerializeField] GameObject gameOverDisplay;

    //timing and display
    private float timer;
    [SerializeField] float disclaimerDuration;
    private bool disclaimerDone = false;
    private bool timerFull = false;
    [SerializeField] float levelTime;
    private bool gameIsOver = false;

    //player enabling
    private GameObject player;
    private Vector3 playerOrigin;
    private bool playerHasMoved = false;
    private float startTimer;
    private bool playerReady = false;

    //sound
    [SerializeField] AudioSource fillTimer;
    [SerializeField] AudioSource timerTick;
    private bool isTicking = false;
    [SerializeField] AudioSource bgMusic;
    [SerializeField] AudioSource winSound;

    // Start is called before the first frame update
    void Start()
    {
        gameOverDisplay.SetActive(false);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerMove>().hasBeenInGoal == true && !gameIsOver) {
            gameIsOver = true;
            winSound.Play();
            bgMusic.Stop();
        }

        EnablePlayer();

        if(playerReady) {
            CheckIfPlayerMoved();
            player.GetComponent<PlayerMove>().canMove = true;
        }

        DisplayDisclaimer();

        if(disclaimerDone && !timerFull) {
            FillUpTimer();
        }

        if(timerFull) {
            timeDisclaimer.SetActive(false);
            disclaimerBG.SetActive(false);
        }

        if(timerFull && playerHasMoved) {
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
        if(timer == 0) {
            fillTimer.Play();
        }

        //increase timer
        timer += 12.5f * Time.deltaTime;
        timerTxt.GetComponent<TMP_Text>().text = "<mspace=21pxem>" + (Mathf.Round(timer * 100f) / 100f).ToString("f2");
        
        if(timer > levelTime) {
            timerTxt.GetComponent<TMP_Text>().text = "" + levelTime;
            timer = levelTime;
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
        if(timer <= 5 && !isTicking) {
            timerTick.Play();
            isTicking = true;
        }

        if(!gameIsOver) {
            timer -= Time.deltaTime;
            timerTxt.GetComponent<TMP_Text>().text = "<mspace=21pxem>" + (Mathf.Round(timer * 100f) / 100f).ToString("f2");
        }

        if(timer <= 0 && !gameIsOver) {
            timerTick.Stop();
            gameIsOver = true;
            GameOver();
        }
    }

    void CheckIfPlayerMoved()
    {
        if(player.transform.position != playerOrigin && !playerHasMoved) {
            playerHasMoved = true;
        }
    }

    void EnablePlayer()
    {
        if(timerFull && !playerReady) {
            playerReady = true;
            playerOrigin = player.transform.position;
        }
    }

    void GameOver()
    {
        gameOverDisplay.SetActive(true);
        player.GetComponent<PlayerMove>().enabled = false;
        bgMusic.Stop();
    }
}
