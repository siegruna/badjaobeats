using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    public AudioSource musicSource;
    public double songBpm = 120;
    public float firstBeatOffset = 0f;

    private double dspSongStartTime;
    private double secPerBeat;

    public float startOffset = 1f;

    void Start()
    {
        secPerBeat = 60.0 / songBpm;
        dspSongStartTime = AudioSettings.dspTime + startOffset;
        musicSource.PlayScheduled(dspSongStartTime);
    }

    public double SongPositionInSeconds()
    {
        return AudioSettings.dspTime - dspSongStartTime - firstBeatOffset;
    }
}