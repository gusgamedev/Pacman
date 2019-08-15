using UnityEngine;

public class SpawnCherry : MonoBehaviour
{
    public GameObject cherry;
    bool canSpawn = true;

    void Start()
    {
        //toda vez que um nivel e iniciado o contador de cerejas e zerado
        GameManager.instance.cherries = 0;
        canSpawn = true;
    }

    private void Update()
    {           
        if (canSpawn)
        {
            //se o jogador ainda nao pegou 3 cerejas o gerador de cerejas e chamado
            if (GameManager.instance.cherries < 3)
            {
                //apos 10 segundos sera gerada uma cereja 
                Invoke("Spawn", 10f);
                //e ira parar o gerador de cerejas
                canSpawn = false;
            }            
        }
    }

    void Spawn()
    {
        //cria a cereja 
        GameObject clone = (GameObject)Instantiate(cherry, transform.position, Quaternion.identity);
        //destroi ela apos 7 segundos caso o jogador nao a pegue
        Destroy(clone, 7f);
        //apos 7 segundos habilita o gerador de cereja novamente 
        Invoke("EnableSpawn", 7f);

    }

    void EnableSpawn()
    {
        canSpawn = true;
    }

    
}
