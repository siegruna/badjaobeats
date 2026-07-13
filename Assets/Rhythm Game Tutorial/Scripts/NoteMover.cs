using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private double hitTime;
    private double travelTime;
    private Vector3 startPos, endPos;
    private RhythmConductor conductor;
    public bool pressable = false;

    public KeyCode keyToPress = KeyCode.A;

    public void Init(double hitTime, RhythmConductor conductor, Vector3 start, Vector3 end, float travelTime)
    {
        this.hitTime = hitTime;
        this.conductor = conductor;
        this.startPos = start;
        this.endPos = end;
        this.travelTime = travelTime;
    }

    void Update()
    {
        double songTime = conductor.SongPositionInSeconds();
        double t = 1.0 - (hitTime - songTime) / travelTime;
        transform.position = Vector3.LerpUnclamped(startPos, endPos, (float)t);

        if (Input.GetKeyDown(keyToPress))
        {
            if (pressable)
            {
                double timeDifference = Math.Abs(songTime - hitTime);
                if (timeDifference <= 0.1)
                {
                    GameManager.Instance.PerfectHit();
                }
                else if (timeDifference <= 0.2)
                {
                    GameManager.Instance.GoodHit();
                }
                Destroy(gameObject);
            }
        }
        else if (songTime - hitTime > 0.2)
        {;
            GameManager.Instance.NoteMissed();
            Destroy(gameObject);
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            pressable = true;
            //Debug.Log("Note entered activator zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            pressable = false;
            //Debug.Log("Note exited activator zone");
        }
    }
}