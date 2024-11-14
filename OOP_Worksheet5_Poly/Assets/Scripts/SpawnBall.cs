using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBall : TimerComp
{
    [SerializeField] GameObject greenBall;
    [SerializeField] GameObject redBall;

    List<GameObject> spawnBalls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spawnBalls.Add(greenBall);
        spawnBalls.Add(redBall);
;        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetTime());
        if (GetTime() >= GetMaxTime()) 
        {
            int rand = Random.Range(0,3);
            Instantiate(spawnBalls[rand]); //idk how to spawn then move the object after
            ResetTimer();
        }
        else
        {
            StartTimer();
        }

    }




}
