using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongPreviewSO", menuName = "ScriptableObjects/SongPreviewSO")]
public class SongPreviewSO : ScriptableObject
{
    public Sprite placard;
    public AudioClip clipPreview;
    public Difficulty difficulty;
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
