using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System;

// Required
[RequireComponent(typeof(Rigidbody2D))]

public class Default_Entity : MonoBehaviour
{

    public EnemiesSO enemiesData;
    public CircleCollider2D deathCollider;

    private int powerX;
    private int powerXval;

    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    protected Animator anim;
    protected SpriteRenderer enemyRender;
    protected ParticleSpawnerManager particleInstance;

    protected bool disableAI;

    protected GameObject entity;

    protected float startPosX;
    protected float startPosY;

    protected bool outOfRange = false;
    protected bool setupOnce = true;

    protected Coroutine colorFlashRoutine;


    //
    protected virtual void Awake()
    {
        entity = this.gameObject;

        startPosX = entity.transform.position.x;
        startPosY = entity.transform.position.y;

        powerXval = 4;
        disableAI = false;
        setupOnce = false;

        anim = GetComponentInChildren<Animator>();
        rb = entity.GetComponent<Rigidbody2D>();
        enemySr = entity.GetComponent<SpriteRenderer>();

        outOfRange = true;

        // stops rotation
        rb.freezeRotation = true;  

    }

    public IEnumerator EnemyDead()
    {

        //To stop from player on detecting them while dying
        entity.tag = "Untagged";

        disableAI = true;

        // Explode Here Effect Here
        anim.SetBool("IsDeath", true);

        entity.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        powerX = Mathf.Sign(entity.GetComponent<Rigidbody2D>().velocity.x) < 0 ? -powerXval : powerXval;
        entity.GetComponent<Rigidbody2D>().velocity = new Vector3(powerX, 3,1);

        yield return new WaitForSeconds(0.7f);
        enemySr.enabled = false;
        entity.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        entity.GetComponent<Rigidbody2D>().gravityScale = 0f;

        GameData.Score += enemiesData.score;
        GameManager.Instance.DisplayScore();

        yield return new WaitForSeconds(1f);
        Destroy(entity);
    }


}
