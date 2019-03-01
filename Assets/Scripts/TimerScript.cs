using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public delegate void TimerCallback();


    public int secondsLeft = 0;

    TimerCallback currentCallback;

    public Animator timerAnimator;
    public Text secondsText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCountdown(int seconds, TimerCallback callback)
    {
        secondsLeft = seconds;
        currentCallback = callback;
        timerAnimator.SetBool("enabled", true);
        StartCoroutine(Countdown());
    }


    IEnumerator Countdown()
    {
        while (secondsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            secondsLeft--;
            timerAnimator.SetTrigger("pulse");
            secondsText.text = secondsLeft.ToString();
        }

        timerAnimator.SetBool("enabled", false);
        // Now that timer is at 0, call it
        currentCallback();
        currentCallback = null;
    }
}
