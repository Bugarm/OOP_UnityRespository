using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FakeWalls : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private bool active = false;

    private bool doOnce = false;
    private Coroutine fadeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doOnce = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBody"))
        { 
            PlayerState.IsFakeWallAllowed = true;

            PlayerState.InFakeWall = true;
        }

        if (collision.CompareTag("PlayerWallDetect") && PlayerState.IsFakeWallAllowed == true)
        {
            active = true;

            if (fadeRoutine == null)
            {
                fadeRoutine = StartCoroutine(FadeEffect());
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("PlayerWallDetect"))
        {
            active = false;
        }

        if (collision.CompareTag("PlayerBody"))
        {
            PlayerState.InFakeWall = false;
        }

    }

    private IEnumerator FadeEffect()
    {

        while (spriteRenderer.color.a >= 0.25f)
        {
            // Fade Out
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.a - Time.deltaTime - 0.005f);
            yield return new WaitForSeconds(0.001f);
        }


        yield return new WaitUntil(() =>  active == false);

        while (spriteRenderer.color.a <= 1f)
        {
            // Fade In
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, spriteRenderer.color.a + Time.deltaTime + 0.005f);
            yield return new WaitForSeconds(0.001f);

        }

        fadeRoutine = null;

    }

    // Debugging purposes 
    private void OnDrawGizmos()
    {
        
        if(doOnce == false)
        { 
            SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, 0.5f);

        }
    }

}
