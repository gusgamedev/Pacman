using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject[] lifes;
    public GameObject[] cherries;

    public Text txtScore;

    // Update is called once per frame
    void Update()
    {
        txtScore.text = GameManager.instance.score.ToString("0000000");

        cherries[0].SetActive(GameManager.instance.cherries > 0);
        cherries[1].SetActive(GameManager.instance.cherries > 1);
        cherries[2].SetActive(GameManager.instance.cherries > 2);

        lifes[0].SetActive(GameManager.instance.lifes > 0);
        lifes[1].SetActive(GameManager.instance.lifes > 1);
        lifes[2].SetActive(GameManager.instance.lifes > 2);
        lifes[3].SetActive(GameManager.instance.lifes > 3);
        lifes[4].SetActive(GameManager.instance.lifes > 4);
    }
}
