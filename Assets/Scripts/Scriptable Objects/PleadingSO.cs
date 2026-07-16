using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PleadingSO", menuName = "ScriptableObjects/PleadingSO")]
public class PleadingSO : ScriptableObject
{
    [Header("Leave empty if this is not a destination node")]
    public string nodeID;

    [TextArea]
    public string description;

    public PleadingChoice[] choices;
}

[System.Serializable]
public struct PleadingChoice
{
    public string choiceText;
    public string nextNodeID;

    public bool isRandom;
    public string randomNodeID;

    [Header("Determines if the node ends with an ending or proceeds to the next level")]
    public string ending;
}

