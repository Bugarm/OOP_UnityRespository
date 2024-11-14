using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;

public class Ball : MonoBehaviour
{

    //1. The impulseVector field is protected so child classes can see it and change its value
    //2. The Start method is virtual and protected so child classes can see it and override it
    //3. The PrintMessage method is virtual and protected so child classes can see it and override it

    protected Vector2 impulseVector;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("hi");
        SetImpulseVector(impulseVector);
    }

    protected void SetImpulseVector(Vector2 impulseVec)
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = impulseVec;

    }
}
