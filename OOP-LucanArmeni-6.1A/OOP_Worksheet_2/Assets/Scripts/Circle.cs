using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject myCirclePrefab;
    public float padding;
    private float XMin,XMax,YMin,YMax;
    // Start is called before the first frame update
    void Start()
    {
        spawnFourCircles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void spawnFourCircles()
    {
        Camera camera = Camera.main;
        XMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        XMax = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        YMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        YMax = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;


        Instantiate(myCirclePrefab, new Vector3(XMin + padding,YMin + padding,0f), Quaternion.identity);
        Instantiate(myCirclePrefab, new Vector3(XMin + padding, YMax - padding, 0f), Quaternion.identity);
        Instantiate(myCirclePrefab, new Vector3(XMax - padding, YMin + padding, 0f), Quaternion.identity);
        Instantiate(myCirclePrefab, new Vector3(XMax - padding, YMax - padding, 0f), Quaternion.identity);

    }
}
