using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyScriptableObject> enemySOList;

    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;

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
        int randomTime = Random.Range(0, 4);
        randomEnemy = Random.Range(0, enemySOList.Count);
        float randomLocation = Random.Range(SpawnPoint1.transform.position.x, SpawnPoint2.transform.position.x);

        yield return new WaitForSeconds(randomTime);

        enemySOList[randomEnemy].enemyGO.GetComponent<Rigidbody2D>().gravityScale = enemySOList[randomEnemy].speed;

        Instantiate(enemySOList[randomEnemy].enemyGO, new Vector2(randomLocation, thisGameobj.y), Quaternion.identity);

        StopCoroutine(enemyRoutine);
        enemyRoutine = null;
    }
}
