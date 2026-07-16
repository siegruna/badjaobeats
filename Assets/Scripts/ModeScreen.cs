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
        if (PlayerPrefs.GetInt("FreeplayUnlocked", 1) == 1)
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
        // Reset the most recent to Level 1 when this is clicked.
        PlayerPrefs.SetInt("Mode", 1);
        PlayerPrefs.SetString("MostRecent", "Level1");
        StartCoroutine(ScreenFader.Instance.FadeOut("SelectScreen"));
    }

    public void BackButton()
    {
        StartCoroutine(ScreenFader.Instance.FadeOut("MainMenu"));
    }
}
