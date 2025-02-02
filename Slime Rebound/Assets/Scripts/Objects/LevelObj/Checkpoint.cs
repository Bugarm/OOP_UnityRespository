using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private bool isChecked;
    // Start is called before the first frame update
    void Start()
    {
        isChecked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isChecked == false && collision.CompareTag("PlayerBody"))
        {
            SaveLoadManager.Instance.SaveDataCheckPoint(this.gameObject.transform.position);
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            isChecked = true;
        }
    }
}
