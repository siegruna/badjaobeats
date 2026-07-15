using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PleadingScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private RectTransform choicePanel;
    [SerializeField]
    private GameObject choiceButtonPrefab;

    // Sprites for art
    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;
    [SerializeField]
    private Sprite sprite3;

    [Header("Titles")]
    [SerializeField] private string title1;
    [SerializeField] private string title2;
    [SerializeField] private string title3;

    [Header("Event Nodes")]
    [SerializeField] private List<PleadingSO> eventNodes = new List<PleadingSO>();

    private int currentNodeIndex = 0;

    private Button button1;
    private Button button2;
    private Button button3;

    private void Start()
    {
        StartCoroutine(ScreenFader.Instance.FadeIn());

        string mostRecentScene = PlayerPrefs.GetString("MostRecent", "SelectScreen");

        if (mostRecentScene == "Level1")
        {
            currentNodeIndex = 0;
            title.text = title1;
        }
        else if (mostRecentScene == "Level2")
        {
            currentNodeIndex = 1;
            title.text = title2;
            
        }
        else if (mostRecentScene == "Level3")
        {
            currentNodeIndex = 2;
            title.text = title3;
        }

        description.text = eventNodes[currentNodeIndex].description;

        // Instantiate choices
        for (int i = 0; i < eventNodes[currentNodeIndex].choices.Length; i++)
        {
            CreateChoice(eventNodes[currentNodeIndex].choices[i], i);
        }
    }

    private void CreateChoice(PleadingChoice choice, int index)
    {
        GameObject newChoice = Instantiate(choiceButtonPrefab, choicePanel.position, Quaternion.identity, choicePanel);
        newChoice.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

        if (index == 0)
        {
            button1 = newChoice.GetComponent<Button>();
            button1.onClick.AddListener(Option1);
        }
        else if (index == 1)
        {
            button2 = newChoice.GetComponent<Button>();
            button2.onClick.AddListener(Option2);
        }
        else
        {
            button3 = newChoice.GetComponent<Button>();
            button3.onClick.AddListener(Option3);
        }

    }

    public void Option1()
    {
        TransitionToEnding(0);

        foreach (PleadingSO node in eventNodes)
        {
            if (node.nodeID == eventNodes[currentNodeIndex].choices[0].nextNodeID)
            {
                currentNodeIndex = eventNodes.IndexOf(node);
                break;
            }

            
        }

        UpdateUI();
    }

    public void Option2()
    {
        TransitionToEnding(1);

        foreach (PleadingSO node in eventNodes)
        {
            if (node.nodeID == eventNodes[currentNodeIndex].choices[1].nextNodeID)
            {
                currentNodeIndex = eventNodes.IndexOf(node);
                break;
            }
        }

        UpdateUI();

    }

    public void Option3()
    {
        TransitionToEnding(2);

        foreach (PleadingSO node in eventNodes)
        {
            if (node.nodeID == eventNodes[currentNodeIndex].choices[2].nextNodeID)
            {
                currentNodeIndex = eventNodes.IndexOf(node);
                break;
            }
        }

        UpdateUI();

    }

    private void UpdateUI() {
        while (choicePanel.childCount > 0)
        {
            DestroyImmediate(choicePanel.GetChild(0).gameObject);
        }

        description.text = eventNodes[currentNodeIndex].description;
        // Instantiate choices
        for (int i = 0; i < eventNodes[currentNodeIndex].choices.Length; i++)
        {
            CreateChoice(eventNodes[currentNodeIndex].choices[i], i);
        }
    }

    private void TransitionToEnding(int choice)
    {
        if (eventNodes[currentNodeIndex].choices[choice].ending == "Good")
        {
            PlayerPrefs.SetInt("Ending", 1);
            ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());
        }
        else if (eventNodes[currentNodeIndex].choices[choice].ending == "Bad")
        {
            PlayerPrefs.SetInt("Ending", 0);
            ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());
        }
        else if (eventNodes[currentNodeIndex].choices[choice].ending == "Continue")
        {
            string lastScene = PlayerPrefs.GetString("MostRecent", "SelectScreen");

            if (lastScene == "Level1")
            {
                SceneManager.LoadSceneAsync("Level2");
            }
            if (lastScene == "Level2")
            {
                SceneManager.LoadSceneAsync("Level3");
            }
        }
    }
}
