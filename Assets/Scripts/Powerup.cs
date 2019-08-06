using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private AudioSource soundPowerUp;

    private void Start()
    {
        soundPowerUp = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            soundPowerUp.Play();

            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");

            foreach (var ghost in ghosts)
            {
                ghost.GetComponent<Enemy>().SetScared();
            }

            GameManager.instance.SetScore(300);
            GameManager.instance.DecreasePills();
            Destroy(gameObject);

        }
    }
}
