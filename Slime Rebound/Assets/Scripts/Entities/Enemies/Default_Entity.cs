using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Default_Entity : MonoBehaviour
{
    private Rigidbody2D enemyVel;

    //
    protected virtual void Awake()
    {
        enemyVel = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        // check when player attacked it [first]
        // check when it hits player
        if(hit.CompareTag("PlayerDamageBox"))
        {
            
            Destroy(this.gameObject);
        }
        else if(hit.CompareTag("Player"))
        {
            GameData.Hp--;
        }

    }
}
