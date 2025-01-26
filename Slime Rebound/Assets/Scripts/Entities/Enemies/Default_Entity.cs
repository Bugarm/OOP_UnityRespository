using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Default_Entity : MonoBehaviour
{
    private CircleCollider2D enemyBody;
    private Rigidbody2D enemyVel;
    private int powerX;
    private int powerXval;
    protected GameObject player;
    private Rigidbody2D playerRB;

    protected bool disableAI;
    public CircleCollider2D deathCollider;

    protected GameObject enemy;

    protected float startPosX;
    protected float startPosY;

    //
    protected virtual void Awake()
    {
        enemy = this.gameObject;

        startPosX = enemy.transform.position.x;
        startPosY = enemy.transform.position.y;

        powerXval = 4;
        disableAI = false;
        enemyVel = GetComponent<Rigidbody2D>();
        enemyBody = this.gameObject.GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator EnemyDead()
    {
        disableAI = true;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? -powerXval : powerXval;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(powerX, 3,1);

        yield return new WaitForSeconds(1);

        // Explode Here
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
    
}
