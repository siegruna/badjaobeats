using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public RhythmConductor conductor;
    public GameObject donPrefab, kaPrefab;
    public Transform hitBar, spawnPoint;
    public float noteTravelTime = 2.0f;
    public RectTransform noteParent;

    public TextAsset chartJson; // drag your .json file into this in the Inspector

    public List<NoteData> chart = new List<NoteData>();
    public Dictionary<NoteData, GameObject> activeNoteObjects = new Dictionary<NoteData, GameObject>();
    private int nextNoteIndex = 0;

    [Header("Adjust this value for calibration")]
    public double startTime = 2.0;

    public void StartSpawner()
    {
        if (chartJson == null)
        {
            Debug.LogError("No chart JSON assigned on NoteSpawner!");
            return;
        }

        ChartFile loadedChart = JsonUtility.FromJson<ChartFile>(chartJson.text);
        chart = loadedChart.notes;

        Debug.Log($"Loaded {chart.Count} notes, BPM: {loadedChart.bpm}");
    }

    void Update()
    {
        double songTime = conductor.SongPositionInSeconds();

        if (nextNoteIndex < chart.Count)
        {
            Debug.Log($"songTime={songTime:F2}, next note time={chart[nextNoteIndex].time:F2}, diff={chart[nextNoteIndex].time - songTime:F2}");
        }

        while (nextNoteIndex < chart.Count &&
               chart[nextNoteIndex].time - songTime <= noteTravelTime)
        {
            SpawnNote(chart[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(NoteData data)
    {
        var prefab = data.GetNoteType() == NoteType.Don ? donPrefab : kaPrefab;
        var noteObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity, noteParent);
        Debug.Log($"Instantiated: {noteObj.name}, instanceID={noteObj.GetInstanceID()}, active={noteObj.activeInHierarchy}");

        if (data == null)
        {
            Debug.Log("data is null");
        }
        else
        {
            Debug.Log("data is not null");
        }
            noteObj.GetComponent<NoteMover>().Init(data, conductor, spawnPoint.position, hitBar.position, noteTravelTime);
        activeNoteObjects[data] = noteObj;
    }

    public void DestroyNoteVisual(NoteData data)
    {
        if (activeNoteObjects.TryGetValue(data, out GameObject obj))
        {
            obj.GetComponent<NoteMover>().DestroyNote();
            activeNoteObjects.Remove(data);
        }
    }
}