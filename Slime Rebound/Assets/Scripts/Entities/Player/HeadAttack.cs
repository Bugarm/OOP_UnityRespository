using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadAttack : Singleton<HeadAttack>
{
    private GameObject head;

    private Coroutine headDestroy;

    protected override void Awake()
    {
        base.Awake();
        head = this.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.CompareTag("Level") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable") || collision.CompareTag("Box") || collision.CompareTag("SkeleChain") || collision.CompareTag("Enemies") || collision.CompareTag("Box") || collision.CompareTag("Obsticales")) && PlayerState.IsHeadThrown == true)
        {
            if(headDestroy == null)
            { 
                headDestroy = StartCoroutine(DestroyHead());
            }
        }
        
        //if(PlayerState.IsHeadThrown == true)
        //{
        //    float moveSlight;
        //    if (head.transform.localScale.x == -1)
        //    {
        //        moveSlight = -1.5f;
        //    }
        //    else
        //    {
        //        moveSlight = 1.5f;
        //    }

        //    head.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSlight, -1.5f);
        //    head.GetComponent<Rigidbody2D>().gravityScale = 14;
        //}

    }

    private void OnBecameInvisible()
    {
        PlayerState.IsHeadThrown = false;
        Destroy(head.transform.parent.gameObject);
    }

    private IEnumerator DestroyHead()
    {
        PlayerState.IsHeadThrown = false;
        head.GetComponent<SpriteRenderer>().sprite = null;
        head.GetComponent<CircleCollider2D>().enabled = false;

        Vector3 look = head.GetComponent<Rigidbody2D>().velocity.x >= 0 ? Vector3.left : Vector3.right;
        head.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, this.gameObject.transform.position, Quaternion.LookRotation(look))); 

        yield return new WaitForSeconds(1f);
        Destroy(head.transform.parent.gameObject);
        headDestroy = null;
    }
}
