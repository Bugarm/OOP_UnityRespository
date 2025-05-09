using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadAttack : Singleton<HeadAttack>
{
    private GameObject head;

    Animator headAnim;

    private Coroutine headDestroy;

    protected override void Awake()
    {
        base.Awake();
        head = this.gameObject;
        headAnim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.CompareTag("Level") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable") || collision.CompareTag("Box") || collision.CompareTag("SkeleChain") || collision.CompareTag("Enemies") || collision.CompareTag("Box") || collision.CompareTag("Obsticales")) && PlayerState.IsHeadThrown == true)
        {
            if(headDestroy == null)
            {
                headAnim.SetBool("HeadSpin", false);
                headDestroy = StartCoroutine(DestroyHead());
            }
        }

    }

    private void OnBecameInvisible()
    {
        if (headDestroy == null && head.activeInHierarchy == true)
        {
            headAnim.SetBool("HeadSpin", false);
            PlayerState.IsHeadThrown = false;
            headDestroy = StartCoroutine(DestroyHead());
        }
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
        headAnim.SetBool("HeadSpin", false);
        Destroy(head.transform.parent.gameObject);
        headDestroy = null;
    }
}
