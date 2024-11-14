using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBall : Ball
{
    // Start is called before the first frame update
    protected override void Start()
    {
        impulseVector = new Vector2(0f, 2f);

        base.Start();

        Debug.Log("Bye");
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

}
