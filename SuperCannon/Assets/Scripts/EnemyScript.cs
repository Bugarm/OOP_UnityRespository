using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    EnemySpawner enemySpawn;
    CannonScontroller cannonHp;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = GetComponent<EnemySpawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("player"))
        {
            cannonHp.Hp -= enemySpawn.enemySOList[enemySpawn.randomEnemy].strength;
        }
    }
}
