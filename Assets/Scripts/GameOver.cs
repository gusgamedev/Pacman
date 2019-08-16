using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text txtScore;
    public Text txtHiScore;

    // Start is called before the first frame update
    void Start()
    {

        txtHiScore.text = "HI-SCORE: " + PlayerPrefs.GetInt("hi-score").ToString("0000000");
        txtScore.text = "SCORE: " + GameManager.instance.score.ToString("0000000");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            GameManager.instance.StartGame();
    }
}
