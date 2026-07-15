using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassengerSO", menuName = "ScriptableObjects/PassengerSO")]
public class PassengerSO : ScriptableObject
{
    public Sprite[] expressions;
    public bool isStatic = false;
    public int position = 0;
}
