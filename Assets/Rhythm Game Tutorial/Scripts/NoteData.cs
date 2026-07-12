using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NoteData
{
    public double time;
    public NoteType type;
    public bool isBig;
    //public bool isPressable;
}

public enum NoteType { Don, Ka }