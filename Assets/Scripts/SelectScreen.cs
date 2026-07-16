using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScreen : MonoBehaviour
{
    public List<SongPreviewSO> songPreviews;

    public int selectedSongIndex = 0;
    public Button selectButton;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip squeakSound;

    [SerializeField] Sprite easySprite;
    [SerializeField] Sprite mediumSprite;
    [SerializeField] Sprite hardSprite;

    [SerializeField] Image difficultyImage;

    void Start()
    {
        string mostRecent = PlayerPrefs.GetString("MostRecent", "Level1");

        selectedSongIndex = mostRecent[^1] - 1 - '0';

        StartCoroutine(ScreenFader.Instance.FadeIn());
        selectButton.GetComponent<Image>().sprite = songPreviews[selectedSongIndex].placard;
        audioSource.clip = songPreviews[selectedSongIndex].clipPreview;
        SetDifficulty(songPreviews[selectedSongIndex].difficulty);

        audioSource.Play();
    }

    public void SelectSong()
    {
        string levelName = "Level" + (selectedSongIndex + 1);
        StartCoroutine (ScreenFader.Instance.FadeOut(levelName));
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Easy)
        {
            difficultyImage.sprite = easySprite;
        }
        else if (difficulty == Difficulty.Medium)
        {
            difficultyImage.sprite = mediumSprite;
        }
        else
        {
            difficultyImage.sprite = hardSprite;
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
        if (selectedSongIndex < songPreviews.Count - 1)
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
        selectButton.GetComponent<Image>().sprite = songPreviews[selectedSongIndex].placard;
        SetDifficulty(songPreviews[selectedSongIndex].difficulty);
        audioSource.clip = songPreviews[selectedSongIndex].clipPreview;
        audioSource.Play();
    }

    public void TetoButton()
    {
        audioSource.PlayOneShot(squeakSound);
    }
}
