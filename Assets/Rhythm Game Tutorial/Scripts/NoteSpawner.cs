using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public RhythmConductor conductor;
    public GameObject donPrefab, kaPrefab;
    public Transform hitBar, spawnPoint;
    public float noteTravelTime = 2.0f;
    public RectTransform gamePanel;

    public List<NoteData> chart = new List<NoteData>();
    private int nextNoteIndex = 0;

    [Header("Adjust this value for calibration")]
    public double startTime = 2.0;

    void Start()
    {
        double interval = 0.4; 
        int noteCount = 60;

        for (int i = 0; i < noteCount; i++)
        {
            NoteType type = (i % 3 == 0) ? NoteType.Ka : NoteType.Don; 
            chart.Add(new NoteData { time = startTime + (i * interval), type = type });
        }

    }

    void Update()
    {
        double songTime = conductor.SongPositionInSeconds();

        while (nextNoteIndex < chart.Count &&
               chart[nextNoteIndex].time - songTime <= noteTravelTime)
        {
            SpawnNote(chart[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(NoteData data)
    {
        Debug.Log("Spawning note: " + data.type + " at time " + data.time);
        var prefab = data.type == NoteType.Don ? donPrefab : kaPrefab;
        var noteObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity, gamePanel);
        noteObj.GetComponent<NoteMover>().Init(data.time, conductor, spawnPoint.position, hitBar.position, noteTravelTime);
    }


}