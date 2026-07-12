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

    void Start()
    {
        secPerBeat = 60.0 / songBpm;
        dspSongStartTime = AudioSettings.dspTime + 1.0;
        musicSource.PlayScheduled(dspSongStartTime);
    }

    public double SongPositionInSeconds()
    {
        return AudioSettings.dspTime - dspSongStartTime - firstBeatOffset;
    }
}