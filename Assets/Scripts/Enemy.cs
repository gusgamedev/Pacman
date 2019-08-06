using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Waypoints")]
    public Waypoint[] routes;
    public Waypoint firstRoute;
    private Waypoint currentRoute;

    [Header("Enemy properties")]
    public float speed = 0.08f;
    public Transform home;

    private int myIndexPoint = 0;
    private SpriteRenderer sprite;
    private Color originalColor;
    private Color scaredColor = new Color(0f, 15f, 186f, 1f);
    private enum State { HUNT, SCARED, DEAD };
    private State enemyState;
    
    // Start is called before the first frame update
    void Start()
    {
        currentRoute = firstRoute;
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        enemyState = State.HUNT;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SetScared();

        //Inimigo se move somente se não estiver morto;
        if (enemyState != State.DEAD)
        {   
            if (
                (transform.position.x != currentRoute.waypoints[myIndexPoint].position.x) ||
                (transform.position.y != currentRoute.waypoints[myIndexPoint].position.y)
            )
            {
                transform.position = Vector2.MoveTowards(transform.position, currentRoute.waypoints[myIndexPoint].position, speed);
            }
            else
            {
                //se inimigo estiver assustado ele faz a rota no sentido contrario
                if (enemyState == State.SCARED)
                {
                    if (myIndexPoint == 0)
                        myIndexPoint = currentRoute.waypoints.Length - 1;
                    else
                        myIndexPoint--;
                }
                else
                {
                    if (myIndexPoint == currentRoute.waypoints.Length - 1)
                        SetRoute();
                    else
                        myIndexPoint++;
                }
            }
        }
    }

    void SetRoute()
    {
        if (enemyState == State.DEAD)
            currentRoute = firstRoute;
        else 
            currentRoute = routes[Random.Range(0, routes.Length)];

        myIndexPoint = 0;
    }

    public void SetScared()
    {
        if (enemyState == State.HUNT)
        {
            enemyState = State.SCARED;
            sprite.color = scaredColor;
            speed -= 0.04f;
            Invoke("SetHunt", 4f);
        }
    }

    void SetHunt()
    {
        enemyState = State.HUNT;
        sprite.color = originalColor;
        speed += 0.04f;        
    }

    void SetDead()
    {   
        enemyState = State.DEAD;
        transform.position = home.position;
        SetRoute();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyState == State.SCARED)
            {
                GameManager.instance.SetScore(1000);
                SetDead();
            } else
            {
                GameManager.instance.GameOver();
            }
        }
    }


}
