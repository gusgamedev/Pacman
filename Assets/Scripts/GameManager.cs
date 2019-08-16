using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lifes = 3;
    public int cherries = 0;
    public int score = 0;

    private int numberPills = 0;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public void SetScore(int pScore)
    {
        //incrementa a pontuacao a cada item coletado ou inimigo destruido
        score += pScore;

        //quando o jogador alcacar as pontuacaoes de 10k, 15k e 20k ele ganha uma vida
        if (score == 10000 || score == 15000 || score == 20000)
            lifes++;

        if (score > PlayerPrefs.GetInt("hi-score"))
            PlayerPrefs.SetInt("hi-score", score);
    }

    public void GetTotalPills()
    {
        //obtem o numero total de Pills para podermos identificar quantas 
        //o jogador precisa coletar para finalizar a fase
        numberPills =  GameObject.FindGameObjectsWithTag("Points").Length;
        numberPills += GameObject.FindGameObjectsWithTag("PowerUp").Length;
    }

    public void DecreasePills()
    {
        //funcao que decrementa cada Pill coletada ate que o jogador colete a ultima
        numberPills--;

        //ao coletar a ultimo o jogador passa de fase;
        if (numberPills <= 0)
            GameOver();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu");        
        cherries = 0;
        lifes = 3;
        score = 0;        
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
