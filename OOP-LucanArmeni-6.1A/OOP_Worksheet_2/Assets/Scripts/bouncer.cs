using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Bouncer : MonoBehaviour
{
    public int totalHealth;
    SpriteRenderer gameObjectCirc;
    private float alpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectCirc = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (totalHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "collCheck")
        {
            Debug.Log(totalHealth + " " + alpha);

<<<<<<< HEAD
            gameObjectCirc.color = new Color(159.0f / 255.0f, 231.0f / 255.0f, 142.0f / 255.0f, alpha);
=======
            gameObjectCirc.color = new Color(255.0f / 255.0f, 87.0f / 255.0f, 87.0f / 255.0f, alpha);
>>>>>>> 5f7b9cd514eed3761cfaf7b1f0bcacc54383d574
            alpha -= 0.1f;
            totalHealth -= 10;

        }
    }

}
