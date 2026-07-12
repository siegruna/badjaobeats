using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private double hitTime;
    private double travelTime;
    private Vector3 startPos, endPos;
    private RhythmConductor conductor;

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

        if (songTime - hitTime > 0.2)
            Destroy(gameObject);
    }
}