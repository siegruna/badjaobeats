using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScreen : MonoBehaviour
{
    public Sprite firstLevelSprite;
    public Sprite secondLevelSprite;
    public Sprite thirdLevelSprite;
    //public Image lockedIcon;

    public int selectedSongIndex = 0;
    public Button selectButton;
    void Start()
    {
        // 1 means Freeplay
        PlayerPrefs.SetInt("Mode", 1);

        StartCoroutine(ScreenFader.Instance.FadeIn());
        selectButton.GetComponent<Image>().sprite = firstLevelSprite;
    }

    public void SelectSong()
    {
        if (selectedSongIndex == 0)
        {
            StartCoroutine(ScreenFader.Instance.FadeOut("Level1"));
        }
        else if (selectedSongIndex == 1)
        {
            StartCoroutine(ScreenFader.Instance.FadeOut("Level2"));
        }
        else
        {

            StartCoroutine(ScreenFader.Instance.FadeOut("Level3"));
        }
    }

    public void PreviousSong()
    {
        if (selectedSongIndex - 1 >= 0)
        {
            selectedSongIndex--;
            UpdateUI();
        }
    }

    public void NextSong()
    {
        if (selectedSongIndex <= 2)
        {
            selectedSongIndex++;
            UpdateUI();
        }
    }

    public void BackButton()
    {
        StartCoroutine(ScreenFader.Instance.FadeOut("ModeScreen"));
    }

    private void UpdateUI()
    {
        if (selectedSongIndex == 0)
        {
            selectButton.gameObject.GetComponent<Image>().sprite = firstLevelSprite;
        }
        else if (selectedSongIndex == 1)
        {
            selectButton.gameObject.GetComponent<Image>().sprite = secondLevelSprite;
        }
        else if (selectedSongIndex == 2)
        {
            selectButton.gameObject.GetComponent<Image>().sprite = thirdLevelSprite;
        }
    }
}
