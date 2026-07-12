using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueSO dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text speakerText, speakerName;
    public Image portrait;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

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
        isDialogueActive = true;
        dialogueIndex = 0;

        speakerName.SetText(dialogueData.speakerName);
        portrait.sprite = dialogueData.speakerPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            speakerText.SetText(dialogueData.dialogueLines[dialogueIndex]);
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
        speakerText.SetText("");

        foreach (char c in dialogueData.dialogueLines[dialogueIndex].ToCharArray())
        {
            speakerText.text += c;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        speakerText.SetText("");
        dialoguePanel.SetActive(false);
    }
}
