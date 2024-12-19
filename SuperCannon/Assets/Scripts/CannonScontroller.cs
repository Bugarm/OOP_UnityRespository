using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonScontroller : Singleton<CannonScontroller>
{
    public static CannonScontroller instance;

    [SerializeField] GameObject bullet1Prefab;
    [SerializeField] GameObject bullet2Prefab;
    [SerializeField] Transform cannonTip;
    [SerializeField] float firingRate1;
    [SerializeField] float firingRate2;

    Quaternion clampRotationLow, clampRotationHigh;

    Coroutine fire1coroutine, fire2coroutine;

    public ObjectPooling cannonBallPool, misslePool;

    // Start is called before the first frame update
    void Start()
    {
        GameData.Score = 0;

        clampRotationLow = Quaternion.Euler(0, 0, -70f);
        clampRotationHigh = Quaternion.Euler(0, 0, +70f);

        
    }

    // Update is called once per frame
    void Update()
    {
        PointatMouse();
        
        FireCannon();

        if (GameData.Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FireCannon()
    {

        if (Input.GetMouseButtonDown(0) && fire1coroutine == null)
        {
            //Instantiate(bullet1Prefab, cannonTip.position, cannonTip.rotation);
           
            if(cannonBallPool != null)
            {
                fire1coroutine = StartCoroutine(FireContinously(cannonBallPool, firingRate1));
            }
        }



        if (Input.GetMouseButtonDown(1) && fire2coroutine == null)
        {
            //Instantiate(bullet2Prefab, cannonTip.position, cannonTip.rotation);
            

            if(misslePool != null)
            {
                fire2coroutine = StartCoroutine(FireContinously(misslePool, firingRate2));
            }

        }

        // Reset coroutine

        if (Input.GetMouseButtonUp(0) && cannonBallPool != null)
        {

            StopCoroutine(fire1coroutine);
            fire1coroutine = null;
        }

        if (Input.GetMouseButtonUp(1) && misslePool != null)
        {

            StopCoroutine(fire2coroutine);
            fire2coroutine = null;
            
            
        }
    }

    IEnumerator FireContinously(ObjectPooling bulletPooled, float _fireRate)
    {
        while (true)
        {
            GameObject pooledBullet = bulletPooled.GetPoolObject();
            pooledBullet.transform.rotation = cannonTip.rotation;
            pooledBullet.transform.position = cannonTip.position;


            //Instantiate(bulletPrefab, cannonTip.position, cannonTip.rotation);
            pooledBullet.SetActive(true);
            

            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void PointatMouse()
    {
        Vector3 relativePos = this.transform.position - GameData.MousePos;
    
        Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.forward);
        newRotation.x = 0f;
        newRotation.y = 0f;
        newRotation.z = Mathf.Clamp(newRotation.z, clampRotationLow.z, clampRotationHigh.z);
        newRotation.w = Mathf.Clamp(newRotation.w, clampRotationLow.w, clampRotationHigh.w);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, Time.deltaTime * 3);
    }

    
}
