using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxColl;

    [SerializeField] private GameObject particleBreak;

    private Animator boxAnimator;

    public bool isDestroyed;

    public int score;

    Coroutine destroyRoutine;

    // Start is called before the first frame update
    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            if(destroyRoutine == null)
            {
                destroyRoutine = StartCoroutine(DestroyAnim());
            }
        }
    }

    private IEnumerator DestroyAnim()
    {
        isDestroyed = true; 

        GameData.Score += score;
        GameManager.Instance.DisplayScore();

        boxColl.isTrigger = true;
        boxColl.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        // Box Destroy Effect here
        boxAnimator.SetBool("isBreak", true);
        GameObject particleBreakObj = Instantiate(particleBreak, this.gameObject.transform.position,Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Destroy(particleBreakObj);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);

    }



}
