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

    public IEnumerator PlayParticle(GameObject particle, Vector2 objPos, Quaternion quat)
    {
        particleGroup = GameObject.Find("ParticleGroup");
        GameObject particleObj = Instantiate(particle, objPos, quat);
        particleObj.transform.parent = particleGroup.transform;
        yield return new WaitForSeconds(1f);
        Destroy(particleObj);
    }

}
