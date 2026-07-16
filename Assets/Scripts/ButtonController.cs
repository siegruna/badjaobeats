using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Image hitBar;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress = KeyCode.Space; // Default key to press is Spacebar
    public KeyCode oppositeKey = KeyCode.Space;

    public Image feedback;

    public Sprite perfectSprite;
    public Sprite goodSprite;
    public Sprite missSprite;

    private void Update()
    {
        if (!DialogueManager.Instance.dialogueActive)
        {
            if (Input.GetKeyDown(keyToPress) && !Input.GetKeyDown(oppositeKey))
            {
                hitBar.sprite = pressedImage;
            }
            else if (Input.GetKeyUp(keyToPress))
            {
                hitBar.sprite = defaultImage;
            }
        }
    }
}
