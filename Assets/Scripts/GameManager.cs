using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int score = 0;
    public Text txtScore;

    //iniciado com 4 pois temos sempre 4 powerups por fase;
    private int numberPills = 4;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Update is called once per frame
    }

    private void Start()
    {
        GetTotalPills();
    }

    public void SetScore(int pScore)
    {
        score += pScore;
        txtScore.text = score.ToString("0000000");
    }

    public void GetTotalPills()
    {
        numberPills += GameObject.FindGameObjectsWithTag("Points").Length;
    }

    public void DecreasePills()
    {
        numberPills--;

        if (numberPills <= 0)
            GameOver();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }



}
