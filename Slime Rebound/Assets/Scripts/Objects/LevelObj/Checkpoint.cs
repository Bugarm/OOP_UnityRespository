using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool isChecked;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
            SaveLoadManager.Instance.SaveDataCheckPoint(this.gameObject.transform.position, SceneManager.GetActiveScene().name);
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleCheckPoint,this.gameObject.transform.position,Quaternion.identity));
            animator.SetBool("IsChecked", true);
            isChecked = true;
        }
    }
}
