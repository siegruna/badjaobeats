using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] dialogueLines;
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;
    public DialogueType dialogueType;
}

[Serializable]
public class DialogueLine
{
    public string speakerName;
    public Sprite speakerPortrait;

    [TextArea]
    public string speakerLine;

    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public string GetName()
    {
        return speakerName;
    }

    public string GetLine()
    {
        return speakerLine;
    }

    public Sprite GetPortrait()
    {
        return speakerPortrait;
    }
}

public enum DialogueType
{
    BeforeGame,
    BadResult,
    MediumResult,
    GoodResult
}
