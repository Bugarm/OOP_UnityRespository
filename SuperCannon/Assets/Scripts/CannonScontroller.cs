using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScontroller : MonoBehaviour
{
    Quaternion clampRotationLow, clampRotationHigh;
    // Start is called before the first frame update
    void Start()
    {
        clampRotationLow = Quaternion.Euler(0, 0, -70f);
        clampRotationHigh = Quaternion.Euler(0, 0, 70f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PointatMouse();
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
