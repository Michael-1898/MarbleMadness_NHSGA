using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToScene(int sceneID)
    {
        if(sceneID == 1 && !SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.GetComponent<ScoreManager>().ClearScore();
        }
        SceneManager.LoadScene(sceneID);
    }
}
