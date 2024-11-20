using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScontroller : MonoBehaviour
{
    [SerializeField] GameObject bullet1Prefab;
    [SerializeField] GameObject bullet2Prefab;
    [SerializeField] Transform cannonTip;
    [SerializeField] float firingRate1;
    [SerializeField] float firingRate2;

    Quaternion clampRotationLow, clampRotationHigh;

    Coroutine fire1coroutine, fire2coroutine;
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

        
        if (Input.GetMouseButtonDown(0) && fire1coroutine == null)
        {
            //Instantiate(bullet1Prefab, cannonTip.position, cannonTip.rotation);
            fire1coroutine = StartCoroutine(FireContinously(bullet1Prefab,firingRate1));
        }
        
       

        if (Input.GetMouseButtonDown(1) && fire2coroutine == null)
        {
            //Instantiate(bullet2Prefab, cannonTip.position, cannonTip.rotation);
            fire2coroutine = StartCoroutine(FireContinously(bullet2Prefab, firingRate2));
        }

        // Reset coroutine

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(fire1coroutine);
            fire1coroutine = null;
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopCoroutine(fire2coroutine);
            fire2coroutine = null;
        }


    }

    IEnumerator FireContinously(GameObject bulletPrefab, float _fireRate)
    {
        while (true)
        {
            Instantiate(bulletPrefab, cannonTip.position, cannonTip.rotation);
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
