using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    public AudioSource musicSource;
    public double songBpm = 120; // will be overwritten if a chart sets it
    public float firstBeatOffset = 0f;

    private double dspSongStartTime;
    private double secPerBeat;

    public void StartMusic()
    {
        secPerBeat = 60.0 / songBpm;
        dspSongStartTime = AudioSettings.dspTime + 1.0;
        musicSource.PlayScheduled(dspSongStartTime);
    }

    public void SetBpm(double bpm)
    {
        songBpm = bpm;
        secPerBeat = 60.0 / songBpm;
    }

    public double SongPositionInSeconds()
    {
        return AudioSettings.dspTime - dspSongStartTime - firstBeatOffset;
    }
}