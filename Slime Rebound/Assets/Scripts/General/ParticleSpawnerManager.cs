using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnerManager : Singleton<ParticleSpawnerManager>
{
    public GameObject particleBreak;
    public GameObject particleBasketWin;
    public GameObject particleSlimeSplash;
    public GameObject particleSlimeBoom;

    private GameObject particleGroup;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayParticle(GameObject particle, Vector2 objPos, Quaternion quat)
    {
        particleGroup = GameObject.Find("ParticleGroup");
        GameObject particleObj = Instantiate(particle, objPos, quat);
        particleObj.transform.parent = particleGroup.transform;
        yield return new WaitForSeconds(1f);
        Destroy(particleObj);
    }

}
