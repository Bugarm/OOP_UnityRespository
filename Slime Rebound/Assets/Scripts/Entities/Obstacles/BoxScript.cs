using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxColl;

    private Animator boxAnimator;

    public bool isDestroyed;

    public int score;

    public bool floorBreak;

    Coroutine destroyRoutine;

    // Start is called before the first frame update
    void Start()
    {
        boxAnimator = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();


        if(this.gameObject.name.StartsWith("FakeWallBreakable"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            bool attackCheck = true;

            // Toggle for breakable floor
            if (floorBreak == true)
            { 
                if (PlayerState.IsPound == true || PlayerState.IsAttackJump == true)
                {
                    attackCheck = true;
                }
                else 
                {
                    attackCheck = false;
                }
            }
            else
            {
                attackCheck = true;
            }

            if (destroyRoutine == null && attackCheck == true)
            {
                destroyRoutine = StartCoroutine(DestroyAnim());
            }
        }
    }

    private IEnumerator DestroyAnim()
    {

        isDestroyed = true;

        if (GameObject.FindAnyObjectByType<GameManager>() != null)
        {
            GameData.Score += score;
            GameManager.Instance.DisplayScore();
        }
        boxColl.isTrigger = true;
        boxColl.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        // Box Destroy Effect here
        boxAnimator.SetBool("isBreak", true);

        yield return new WaitForSeconds(0.1f);
        boxColl.enabled = false;


        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);

    }



}
