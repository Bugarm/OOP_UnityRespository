using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnerManager : Singleton<ParticleSpawnerManager>
{
    //public GameObject toParentTo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayParticle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayParticle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
