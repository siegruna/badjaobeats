using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScreen : MonoBehaviour
{
    public List<Sprite> levelSprites;
    //public Image lockedIcon;

    public int selectedSongIndex = 0;
    public Button selectButton;
    void Start()
    {
        // 1 means Freeplay
        PlayerPrefs.SetInt("Mode", 1);

        StartCoroutine(ScreenFader.Instance.FadeIn());
        selectButton.GetComponent<Image>().sprite = levelSprites[0];
    }

    public void SelectSong()
    {
        string levelName = "Level" + (selectedSongIndex + 1);
        StartCoroutine (ScreenFader.Instance.FadeOut(levelName));
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
        if (selectedSongIndex < levelSprites.Count - 1)
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
        selectButton.gameObject.GetComponent<Image>().sprite = levelSprites[selectedSongIndex];
    }
}
