using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    EnemySpawner enemySpawn;

    public int strength;
    public int hitpoints;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            hitpoints -= 1;
        }

        if (hitpoints <= 0)
        {
            GameData.Score += 1;
            Debug.Log("Score: " +  GameData.Score.ToString());
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "OutofBounds")
        {
            GameData.Hp -= strength;
            Debug.Log("Player Health: " + GameData.Hp.ToString());
            Destroy(gameObject);
        }
    }
}
