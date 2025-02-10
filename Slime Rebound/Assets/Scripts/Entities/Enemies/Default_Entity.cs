using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Default_Entity : MonoBehaviour
{

    public EnemiesSO enemiesData;

    public CircleCollider2D deathCollider;

    private CircleCollider2D enemyBody;
    private Rigidbody2D enemyVel;
    private int powerX;
    private int powerXval;
    protected GameObject player;
    protected Rigidbody2D playerRB;

    protected bool disableAI;

    protected GameObject enemy;

    protected float startPosX;
    protected float startPosY;

    protected Coroutine colorFlashRoutine;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator EnemyDead()
    {
        //To stop from player on detecting them while dying
        enemy.tag = "Untagged";

        disableAI = true;

        StartCoroutine(GameManager.Instance.ColorFlash(enemy));

        enemy.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? -powerXval : powerXval;
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector3(powerX, 3,1);
        
        yield return new WaitForSeconds(1);

        // Explode Here Effect Here


        yield return new WaitForSeconds(0.3f);

        GameData.Score += enemiesData.score;
        GameManager.Instance.DisplayScore();
        StopCoroutine(GameManager.Instance.ColorFlash(enemy));
        Destroy(enemy);
    }


}
