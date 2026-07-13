using System;

[Serializable]
public class NoteData
{
    public double time;
    public string type; 
    public bool isBig;

    public NoteType GetNoteType()
    {
        return type == "Don" ? NoteType.Don : NoteType.Ka;
    }
}

public enum NoteType { Don, Ka }