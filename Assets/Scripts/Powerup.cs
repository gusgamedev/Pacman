using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public int scorePowerup = 300;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");

            foreach (var ghost in ghosts)
            {
                ghost.GetComponent<Enemy>().SetScared();
            }

            GameManager.instance.SetScore(scorePowerup);
            GameManager.instance.DecreasePills();
            Destroy(gameObject);

        }
    }
}
