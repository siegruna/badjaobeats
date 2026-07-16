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
    public List<AudioClip> songs;

    public int selectedSongIndex = 0;
    public Button selectButton;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip squeakSound;
    void Start()
    {
        string mostRecent = PlayerPrefs.GetString("MostRecent", "Level1");

        selectedSongIndex = mostRecent[^1] - 1 - '0';

        StartCoroutine(ScreenFader.Instance.FadeIn());
        selectButton.GetComponent<Image>().sprite = levelSprites[0];
        audioSource.clip = songs[0];
        audioSource.Play();
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
        audioSource.clip = songs[selectedSongIndex];
        audioSource.Play();
    }

    public void TetoButton()
    {
        audioSource.PlayOneShot(squeakSound);
    }
}
