using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Default_Entity : MonoBehaviour
{

    public EnemiesSO enemiesData;
    public CircleCollider2D deathCollider;

    private int powerX;
    private int powerXval;

    private Animator deathAnim;
    private SpriteRenderer enemyRender;
    private ParticleSpawnerManager particleInstance;

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

        deathAnim = GetComponent<Animator>();
        enemyRender = GetComponent<SpriteRenderer>();
        
    }

    public IEnumerator EnemyDead()
    {
        Player.Instance.failedBounces += 2;

        //To stop from player on detecting them while dying
        enemy.tag = "Untagged";

        disableAI = true;

        // Explode Here Effect Here
        deathAnim.SetBool("IsDeath", true);

        enemy.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        powerX = Mathf.Sign(enemy.GetComponent<Rigidbody2D>().velocity.x) < 0 ? -powerXval : powerXval;
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector3(powerX, 3,1);

        yield return new WaitForSeconds(0.7f);
        enemyRender.enabled = false;
        enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        enemy.GetComponent<Rigidbody2D>().gravityScale = 0f;

        GameData.Score += enemiesData.score;
        GameManager.Instance.DisplayScore();

        yield return new WaitForSeconds(1f);
        Destroy(enemy);
    }


}
