using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyScript
{
    Transform enemyObj;
    Rigidbody2D enemyRig2D;
    public List<float> direction = new List<float>();

    Coroutine moveRoutine;

    // Start is called before the first frame update
    void Start()
    {
        enemyRig2D = this.gameObject.GetComponent<Rigidbody2D>();
        enemyObj = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveRoutine == null)
        {
            moveRoutine = StartCoroutine(MoveRandomly());
        }
    }

    IEnumerator MoveRandomly()
    {
        while(true)
        {
            int randomDirection = Random.Range(0, 2);
            float randomTime = Random.Range(0.5f,1.8f);

            yield return new WaitForSeconds(randomTime);
            enemyRig2D.gravityScale = 0;
            enemyRig2D.velocity = new Vector2(direction[randomDirection], 0f);
            
            yield return new WaitForSeconds(1);
            enemyRig2D.velocity = new Vector2(0f, 0f);
            enemyRig2D.gravityScale = speed;
        }
        
    }
}
