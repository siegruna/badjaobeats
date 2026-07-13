using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJudge : MonoBehaviour
{
    public RhythmConductor conductor;
    public NoteSpawner spawner;
    private const float MISS_WINDOW = 0.15f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
            TryHit(NoteType.Don);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
            TryHit(NoteType.Ka);
    }

    void TryHit(NoteType pressedType)
    {
        if (spawner.chart.Count == 0) return;

        double songTime = conductor.SongPositionInSeconds();
        NoteData nearest = null;
        double nearestDelta = double.MaxValue;

        foreach (var note in spawner.chart)
        {
            double delta = System.Math.Abs(songTime - note.time);
            if (delta < nearestDelta)
            {
                nearestDelta = delta;
                nearest = note;
            }
        }

        if (nearest == null || nearestDelta > MISS_WINDOW) return;

        if (nearest.GetNoteType() != pressedType)
        {
            Debug.Log("Miss (wrong key)");
        }
        else
        {
          
        }

        spawner.chart.Remove(nearest);
    }
}