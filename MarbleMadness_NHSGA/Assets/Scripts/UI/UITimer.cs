using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField] GameObject timerTxt;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        timerTxt.GetComponent<TMP_Text>().text = "Timer: " + Mathf.Round(timer * 100f) / 100f;
    }
}
