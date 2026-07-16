using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image endingImage;

    [SerializeField]
    private TMP_Text endingTitle;

    [SerializeField]
    private TMP_Text endingDescription;

    [SerializeField]
    private RectTransform buttonPanel;

    [SerializeField]
    private Button creditsButton;

    [SerializeField]
    private Sprite badEndingSprite;

    [SerializeField]
    private Sprite goodEndingSprite;

    [SerializeField]
    private string badEndingTitle;

    [SerializeField]
    [TextArea] private string badEndingDescription;

    [SerializeField]
    private string goodEndingTitle;

    [SerializeField]
    [TextArea] private string goodEndingDescription;
    void Start()
    {
        if (PlayerPrefs.GetInt("Ending", 0) == 0) // Bad End
        {
            endingImage.sprite = badEndingSprite;
            endingTitle.text = badEndingTitle;
            endingDescription.text = badEndingDescription;

            creditsButton.gameObject.SetActive(false);
            buttonPanel.gameObject.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Ending", 0) == 1) // Good End
        {
            endingImage.sprite = goodEndingSprite;
            endingTitle.text = goodEndingTitle;
            endingDescription.text = goodEndingDescription;

            creditsButton.gameObject.SetActive(true);
            buttonPanel.gameObject.SetActive(false);
        }

        ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeIn());
    }

    public void Retry()
    {
        string mostRecentScene = PlayerPrefs.GetString("MostRecent", "SelectScreen");
        StartCoroutine(ScreenFader.Instance.FadeOut(mostRecentScene));
    }

    public void GiveUp()
    {
        StartCoroutine(ScreenFader.Instance.FadeOut("MainMenu"));
    }
}
