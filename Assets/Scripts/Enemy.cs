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
    public float speed = 6f;
    public Transform home;
    public int scoreScared = 1000;
    


    private AudioSource soundGhost;
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
        soundGhost = GetComponent<AudioSource>();
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
            //verifica a atualposicao do do inimigo nos eixos X e Y em comparacao ao proximo Waypoint
            if (
                (transform.position.x != currentRoute.waypoints[myIndexPoint].position.x) ||
                (transform.position.y != currentRoute.waypoints[myIndexPoint].position.y)
            )
            {
                // se o inimigo ainda nao chegou a proximo waypoint ele continua seu movimento
                transform.position = Vector2.MoveTowards(transform.position, currentRoute.waypoints[myIndexPoint].position, speed * Time.deltaTime);
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
                //se o inimigo chegou ao waypoint ele incrementa e passa para o proximo
                else
                {
                    //se ele estava no ulimo waypoint da lista
                    //sistema seta uma nova rota de forma aleatoria
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
        //se o inimigo foi morto a proxima rota para o inimigo sera a sua primeira rota
        if (enemyState == State.DEAD)
            currentRoute = firstRoute;
        else 
            //caso contrario ele ira atribuir uma rota aleatoria, detre as disponiveis
            currentRoute = routes[Random.Range(0, routes.Length)];

        myIndexPoint = 0;
    }

    public void SetScared()
    {   
        //somente passamos o inimigo para o estado de assustado se ele estiver cacando
        if (enemyState == State.HUNT)
        {
            enemyState = State.SCARED;
            sprite.color = scaredColor;
            speed -= 3f;
            Invoke("SetHunt", 5f);
        }
    }

    void SetHunt()
    {
        enemyState = State.HUNT;
        sprite.color = originalColor;
        speed += 3f;        
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
            //quando inimigo colido com jogador quando esta assutado
            //ele muda para estado de morto
            if (enemyState == State.SCARED)
            {
                GameManager.instance.SetScore(scoreScared);
                soundGhost.Play();
                SetDead();
            }
            //caso ele colida com o jogador no estado cacando            
            else
            {
                //jogador perde 1 vida
                GameManager.instance.lifes--;

                //se vida chega a zero jogo termina
                if (GameManager.instance.lifes <= 0)
                    GameManager.instance.GameOver();
                
                //caso contrario jogador continua tentado
                else                    
                    TryAgain();
            }
        }
    }

    public void TryAgain()
    {
        //a funcao tryagain vai reiniciar a posicao dos inimigos e do jogador 
        //vai dar um pause durante a morte do jogador
        //sera chamada a funcao ResetLevel() para reposicionar o jogador e inimigos
        //ela e chamada dentro de um Corrotina para que o jogo fique pausado por um 
        //tempo e depois o jogo recomeca
        StartCoroutine(ResetLevel());
        Time.timeScale = 0;
    }

    IEnumerator ResetLevel()
    {
        //obtem informacao do objeto jogador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        //toca a musica de morte do player
        player.GetComponent<Player>().SetSound(player.GetComponent<Player>().deathFx);

        //mante o jogo pausado por 1.2 segundos
        yield return new WaitForSecondsRealtime(1.2f);

        //reseta o jogador na fase
        Transform playerStartPos = GameObject.FindGameObjectWithTag("PlayerStartPosition").GetComponent<Transform>();        
        player.transform.position = playerStartPos.position;
        player.GetComponent<Player>().SetSound(player.GetComponent<Player>().chompFx);

        //obtem o objeto de todosos inimigos da cena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Ghost");

        //cada inimgo e resetado na cena da mesma forma que quando ele e morto 
        //e forcamos e ele para o estado de cacador
        foreach (GameObject ghost in enemies)
        {
            ghost.GetComponent<Enemy>().SetDead();
            ghost.GetComponent<Enemy>().enemyState = State.HUNT;
        }

        //removemos o estado de pausa do jogo
        Time.timeScale = 1;



    }


}
