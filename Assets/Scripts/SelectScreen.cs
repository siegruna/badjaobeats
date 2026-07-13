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

    // placeholder
    public TMP_Text levelName;

    public int selectedSongIndex = 0;
    public Button selectButton;
    void Start()
    {
        levelName.text = "Tutorial";
    }

    public void SelectSong()
    {
        if (selectedSongIndex == 0)
        {
            SceneManager.LoadSceneAsync("Level1");
        }
        else if (selectedSongIndex == 1)
        {
            SceneManager.LoadSceneAsync("Level2");
        }
        else
        {
            SceneManager.LoadSceneAsync("Level3");
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

    private void UpdateUI()
    {
        if (selectedSongIndex == 0)
        {
            levelName.text = "Tutorial";
            selectButton.gameObject.GetComponent<Image>().sprite = firstLevelSprite;
            selectButton.interactable = true;
            //lockedIcon.enabled = false;
        }
        else if (selectedSongIndex == 1)
        {
            levelName.text = "Level 2";
            selectButton.gameObject.GetComponent<Image>().sprite = secondLevelSprite;

            if (PlayerPrefs.GetInt("Level2Unlocked", 0) == 1)
            {
                selectButton.interactable = true;
                //lockedIcon.enabled = false;
            }
            else
            {
                selectButton.interactable = false;
                //lockedIcon.enabled = true;
            }
        }
        else if (selectedSongIndex == 2)
        {
            levelName.text = "Level 3";
            selectButton.gameObject.GetComponent<Image>().sprite = thirdLevelSprite;

            if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 1)
            {
                selectButton.interactable = true;
                //lockedIcon.enabled = false;
            }
            else
            {
                selectButton.interactable = false;
                //lockedIcon.enabled = true;
            }
        }
    }
}
