using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public void ApplyDamage(int hitpoints);
}

public class EnemyScript : MonoBehaviour
{
    EnemySpawner enemySpawn;

    public int strength;
    public int hitpoints;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            GetComponent<ITakeDamage>().ApplyDamage(hitpoints);
        }

    }

    private void OnBecameInvisible()
    {
        GameData.Hp -= 1;
        Debug.Log("Player health: " + GameData.Hp);
        Destroy(this.gameObject);
    }
}
