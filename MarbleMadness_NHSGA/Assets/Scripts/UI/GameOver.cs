using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] AudioSource levelFailSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        levelFailSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
