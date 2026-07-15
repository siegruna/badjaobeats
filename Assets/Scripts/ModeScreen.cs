using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeScreen : MonoBehaviour
{
    [SerializeField] private Button freeplayButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FreeplayUnlocked", 0) == 1)
        {
            freeplayButton.interactable = true;
        }
        else
        {
            freeplayButton.interactable = false;
        }

        StartCoroutine(ScreenFader.Instance.FadeIn());
    }

    public void StoryMode()
    {
        // 0 means story mode
        PlayerPrefs.SetInt("Mode", 0);
        StartCoroutine(ScreenFader.Instance.FadeOut("Level1"));
    }

    public void FreePlay()
    {
        StartCoroutine(ScreenFader.Instance.FadeOut("SelectScreen"));
    }

    public void BackButton()
    {
        StartCoroutine(ScreenFader.Instance.FadeOut("MainMenu"));
    }
}
