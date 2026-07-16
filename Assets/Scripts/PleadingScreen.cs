using System;
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

        if (mostRecentScene == "Level2")
        {
            currentNodeIndex = 0;
        }
        else if (mostRecentScene == "Level3")
        {
            currentNodeIndex = 1;
            
        }
        else if (mostRecentScene == "Level4")
        {
            currentNodeIndex = 2;
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
        bool hasEnding = TransitionToEnding(0);

        if (hasEnding)
        {
            return;
        }

        if (eventNodes[currentNodeIndex].choices[0].isRandom)
        {
            int r = UnityEngine.Random.Range(0, 2);
            if (r > 0)
            {
                eventNodes[currentNodeIndex].choices[0].nextNodeID = eventNodes[currentNodeIndex].choices[0].randomNodeID;
            }
        }

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
        bool hasEnding = TransitionToEnding(1);

        if (hasEnding)
        {
            return;
        }


        if (eventNodes[currentNodeIndex].choices[1].isRandom)
        {
            int r = UnityEngine.Random.Range(0, 2);
            if (r > 0)
            {
                eventNodes[currentNodeIndex].choices[1].nextNodeID = eventNodes[currentNodeIndex].choices[1].randomNodeID;
            }
        }


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
        bool hasEnding = TransitionToEnding(2);

        if (hasEnding)
        {
            return;
        }


        if (eventNodes[currentNodeIndex].choices[2].isRandom)
        {
            int r = UnityEngine.Random.Range(0, 2);
            if (r > 0)
            {
                eventNodes[currentNodeIndex].choices[2].nextNodeID = eventNodes[currentNodeIndex].choices[2].randomNodeID;
            }
        }

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

    private bool TransitionToEnding(int choice)
    {
        if (eventNodes[currentNodeIndex].choices[choice].ending == "Good")
        {
            PlayerPrefs.SetInt("Ending", 1);
            StartCoroutine(ScreenFader.Instance.FadeOut());
            return true;
        }
        else if (eventNodes[currentNodeIndex].choices[choice].ending == "Bad")
        {
            PlayerPrefs.SetInt("Ending", 0);
            StartCoroutine(ScreenFader.Instance.FadeOut());
            return true;
        }
        else if (eventNodes[currentNodeIndex].choices[choice].ending == "Continue")
        {
            string lastScene = PlayerPrefs.GetString("MostRecent", "SelectScreen");
            Debug.Log(lastScene);

            if (lastScene == "Level2")
            {
                StartCoroutine(ScreenFader.Instance.FadeOut("Level3"));
            }
            else if (lastScene == "Level3")
            {
                StartCoroutine(ScreenFader.Instance.FadeOut("Level4"));
            }
            return true;
        }
        return false;
    }
}
