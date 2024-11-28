using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyScriptableObject> enemySOList;

    //public GameObject SpawnPoint1;
    //public GameObject SpawnPoint2;

    public float enemySpawnInterval = 1f;

    private Coroutine enemyRoutine;

    private Vector2 thisGameobj;

    public int randomEnemy;

    // Start is called before the first frame update
    void Start()
    {
        thisGameobj = this.gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyRoutine == null)
        {
            enemyRoutine = StartCoroutine(SpawnRandomEnemy());
        }
    }

    IEnumerator SpawnRandomEnemy()
    {
        while(true)
        {
            int randomTime = Random.Range(3, 6);
            randomEnemy = Random.Range(0, enemySOList.Count);
            //float randomLocation = Random.Range(SpawnPoint1.transform.position.x, SpawnPoint2.transform.position.x);
            float randomLocation = Random.Range(GameData.XMin, GameData.XMax);
            Vector3 enemyPos = new Vector3(randomLocation, GameData.XMax, 0);

            GameObject enemyInstance = Instantiate(enemySOList[randomEnemy].enemyGO, enemyPos, Quaternion.identity);
            enemyInstance.GetComponent<EnemyScript>().strength = enemySOList[randomEnemy].strength;
            enemyInstance.GetComponent<EnemyScript>().speed = enemySOList[randomEnemy].speed;
            enemyInstance.GetComponent<EnemyScript>().hitpoints = enemySOList[randomEnemy].hitpoints;

            yield return new WaitForSeconds(enemySpawnInterval);

            
        }
        
    }


}
