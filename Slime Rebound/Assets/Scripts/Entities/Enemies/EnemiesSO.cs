using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]

public class EnemiesSO : ScriptableObject
{

    public GameObject enemyPrefab;

    public float speed;
    public int score;
    
}
