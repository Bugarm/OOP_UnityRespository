using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    Rigidbody2D circleRB;
    [SerializeField] float ySpeed;

    ShapeMovement myCircleMovement = new ShapeMovement(0f,-3f,0f,2f);

    // Start is called before the first frame update
    void Start()
    {
        circleRB = GetComponent<Rigidbody2D>();
        myCircleMovement.ResetPosition(circleRB);
    }

    // Update is called once per frame
    void Update()
    {
        myCircleMovement.MoveUp(circleRB);
        myCircleMovement.MoveDown(circleRB);

    }
}
