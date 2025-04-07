using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FoodItem : MonoBehaviour
{
    public AudioSource collectSFX;

    private SpriteRenderer foodRender;

    private Coroutine routineFood;

    void Start()
    {
        foodRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            if (routineFood == null)
            {
                routineFood = StartCoroutine(CollectDelay());
            }
        }
    }

    private IEnumerator CollectDelay()
    {
        StartCoroutine(AudioManager.Instance.PlaySFXManual(collectSFX, this.gameObject.transform.position));
        GameData.Hp += 1;
        GameManager.Instance.DisplayHp();
        foodRender.enabled = false;
        yield return new WaitForSeconds(collectSFX.clip.length);
        Destroy(this.gameObject);
    }

}
