using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int customScore;

    private SpriteRenderer scoreRender;

    private Coroutine routineScore;

    public AudioSource collectSFX;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = this.gameObject;

        scoreRender = obj.GetComponent<SpriteRenderer>();

        if (customScore >= 100)
        {
            obj.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        }
        else if (customScore >= 50)
        {
            obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        else if (customScore <= 5)
        {
            obj.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            if(routineScore == null)
            {
                routineScore = StartCoroutine(CollectDelay());
            }
        }
    }

    private IEnumerator CollectDelay()
    {
        StartCoroutine(AudioManager.Instance.PlaySFXManual(collectSFX, this.gameObject.transform.position));
        GameData.Score += customScore;
        GameManager.Instance.DisplayScore();
        scoreRender.enabled = false;
        yield return new WaitForSeconds(collectSFX.clip.length);
        Destroy(this.gameObject);
    }

}
