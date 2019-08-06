using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask wallLayer;
    Vector2 movement = Vector2.zero;
    Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveY = Input.GetAxisRaw("Vertical");
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveY > 0) 
            SetMovement(Vector2.up);
        else if (moveY < 0)
            SetMovement(Vector2.down);
        else if (moveX > 0)
            SetMovement(Vector2.right);
        else if (moveX < 0)
            SetMovement(Vector2.left);

        rb2D.velocity = movement * speed;
    }

    void SetMovement(Vector2 pMovement)
    {
        //so permite que o pacman troque de movimento se o lado escolhido não colidir com uma parede
        if (CanMove(pMovement))
        {
            //caso o movimento seja na verical, centralizamos o pacman no eixo X
            //para que ele fique posicinado exatamente no centro da entrada do corredor do eixo Y
            if (pMovement.y != 0)
                transform.position = new Vector2(Mathf.Round(transform.position.x), transform.position.y);
            //caso o movimento seja na horizantal, centralizamos o pacman no eixo Y
            //para que ele fique posicinado exatamente no centro da entrada do corredor do eixo X
            if (pMovement.x != 0)
                transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y));

            //mudamos a face do pacman para o sentido do movimento
            transform.right = pMovement;

            //mudamos a direcao do movimento
            movement = pMovement;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        //faz a deteccao da colisao com a parede na direcao que o jogador fizer o movimento
        RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position, (Vector2)transform.position + direction, wallLayer);
        //caso ele nao encotre a colisaão com a parede o retrono sera TRUE permitindo a troca do movimento.
        return (hit.collider == null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Points"))
        {
            GameManager.instance.SetScore(100);
            GameManager.instance.DecreasePills();
            Destroy(collision.gameObject);
        }
    }
}


