using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// didn't use so far
public class CustomTimer : MonoBehaviour
{

    public float duration;
    float elapsedTime;

    [SerializeField]
    private bool pause;


    public delegate void OnTimeSignature(string value);
    private OnTimeSignature OnTimer;

    // Use this for initialization
    void Start()
    {
        OnTimer += FunctionA;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
            {
                elapsedTime -= duration;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartTimer(3.0f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StopTimer();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PauseTimer();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResumeTimer();
        }
    }

    public void StartTimer(float _duration)
    {
        duration = _duration;
        elapsedTime = 0;
        pause = false;
    }
    public void StopTimer()
    {
        duration = 0;
        elapsedTime = 0;
        pause = false;
    }

    public void PauseTimer()
    {
        pause = true;
    }

    public void ResumeTimer()
    {
        pause = false;
    }
    public void FunctionA(string value)
    {
        print(value);
    }
}
