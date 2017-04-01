using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class Timer : MonoBehaviour {
    protected Text timer;
    protected float timer_f;
    public static string text;
    // Use this for initialization
    void Start () {
        timer = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update() {
        if (Board.win) { //if win stop.
            timer.text = null;
            return;
        }
        timer_f += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(timer_f); 
        text = string.Format("{0:D2}:{1:D2}", (int)timespan.TotalMinutes, timespan.Seconds);
        timer.text = text;
    }
}
