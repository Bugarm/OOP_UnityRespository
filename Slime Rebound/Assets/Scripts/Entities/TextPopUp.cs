using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    [SerializeField] private bool doColor = true;

    private SpriteRenderer render;
    private bool active = false;

    private Coroutine fadeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.color = new Color(render.color.r, render.color.g, render.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("TextRange"))
        {
            active = true;

            if(fadeRoutine == null)
            {
                fadeRoutine = StartCoroutine(FadeEffect());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TextRange"))
        {
            active = false;
        }
    }

    private IEnumerator FadeEffect()
    {
        Color color;
        if (doColor == true)
        { 
            color = new Color(0.5f, 1f, 0.5f);
        }
        else
        {
            color = render.color;
        }

        while (render.color.a <= 1f)
        {
            // Fade In
            render.color = new Color(color.r, color.g, color.b, render.color.a + Time.deltaTime + 0.005f);
            yield return new WaitForSeconds(0.001f);

        }

        yield return new WaitUntil(() => active == false);

        while (render.color.a >= 0)
        {
            // Fade Out
            render.color = new Color(color.r, color.g, color.b, render.color.a - Time.deltaTime - 0.005f);
            yield return new WaitForSeconds(0.001f);
        }

        fadeRoutine = null;

    }
}
