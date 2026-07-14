using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueSO dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text speakerText, speakerName;
    public Image portrait;

    private int dialogueIndex;
    private bool isTyping;

    public bool dialogueActive = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            // testing for dialogue
            StartDialogue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called whenever a dialogue needs to happen
    public void StartDialogue()
    {
        dialogueActive = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            speakerText.SetText(dialogueData.dialogueLines[dialogueIndex].GetLine());
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        speakerName.SetText(dialogueData.dialogueLines[dialogueIndex].GetName());
        portrait.sprite = dialogueData.dialogueLines[dialogueIndex].GetPortrait();
        speakerText.SetText("");

        foreach (char c in dialogueData.dialogueLines[dialogueIndex].GetLine())
        {
            speakerText.text += c;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        NextLine();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        speakerText.SetText("");
        dialoguePanel.SetActive(false);

        if (dialogueData.dialogueType == DialogueType.BeforeGame)
        {
            GameManager.Instance.StartGame();
            dialogueActive = false;
            StartCoroutine(ScreenFader.Instance.FadeOut());
        }
        else if (dialogueData.dialogueType == DialogueType.BadResult)
        {
            // Proceed to the bad ending directly
            PlayerPrefs.SetInt("Ending", 0);
            StartCoroutine(ScreenFader.Instance.FadeOut());
        }
        else if (dialogueData.dialogueType == DialogueType.MediumResult)
        {
            // Begging simulator T_T 
            // Depending on the result, either present bad ending or proceed to next stage (make this a new scene or something i guess).
        }
        else
        {
            // Proceed to the next stage.
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                SceneManager.LoadSceneAsync("Level2");
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                SceneManager.LoadSceneAsync("Level3");
            }
            else if (SceneManager.GetActiveScene().name == "Level3")
            {
                // Game completed, go to ending + credits
                // Note: Add good ending here

                PlayerPrefs.SetInt("Ending", 1);
                StartCoroutine(ScreenFader.Instance.FadeOut());
            }
        }
    }
}
