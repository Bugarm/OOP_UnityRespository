using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName =
"ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{

    public GameObject enemyGO;
    public int hitpoints;
    public int strength;
    public float speed;
    
}
