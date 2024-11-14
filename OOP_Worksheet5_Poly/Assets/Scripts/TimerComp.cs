using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerComp : MonoBehaviour
{
    [SerializeField] protected float Maxtimer;
    protected float timer;

    protected float GetTime()
    {
        return timer;
    }

    protected float GetMaxTime()
    {
        return Maxtimer;
    }

    protected void StartTimer()
    {
        timer += 0.01f;
    }

    protected void ResetTimer()
    {
        timer = 0f;
    }
}
