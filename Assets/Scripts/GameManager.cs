using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentScore;
    public float currentMultiplier = 1;
    public int scorePerNote = 10;

    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text multiplierText;

    public Image perfectIndicator;
    public Image goodIndicator;
    public Image missIndicator;

    private int currentCombo = 0;

    public NoteSpawner noteSpawner;
    public RhythmConductor rhythmConductor;

    public Image passengers;
    public Sprite passengerState1;
    public Sprite passengerState2;
    public Sprite passengerState3;

    public List<int> milestones = new List<int> { 1000, 2000};

    [Header("Dialogue triggered at the end of a song based on the result (3 outcomes)")]
    public DialogueSO[] dialogues;

    public void Start()
    {
        Instance = this;

        scoreText.text = "Score: 0";
        comboText.text = "Combo: 0";
        multiplierText.text = "Multiplier: x1";
        
        perfectIndicator.gameObject.SetActive(false);
        goodIndicator.gameObject.SetActive(false);
        missIndicator.gameObject.SetActive(false);

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return ScreenFader.Instance.FadeIn();

        if (PlayerPrefs.GetInt("Mode", 0) == 0)
        {
            DialogueManager.Instance.StartDialogue();
        }
        else
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("MostRecent", SceneManager.GetActiveScene().name);

        noteSpawner.StartSpawner();
        rhythmConductor.StartMusic();
    }

    public void FinishGame()
    {
        Debug.Log("Music stopped playing.");
        if (currentScore < milestones[0])
        {
            // Bad end
            DialogueManager.Instance.dialogueData = dialogues[0];
        } else if (currentScore < milestones[1])
        {
            // Begging sim
            DialogueManager.Instance.dialogueData = dialogues[1];
        } else
        {
            // Good end
            DialogueManager.Instance.dialogueData = dialogues[2];

            UnlockLevel();
        }

        // Only start dialogue if in story mode.
        if (PlayerPrefs.GetInt("Mode", 0) == 0)
        {
            DialogueManager.Instance.StartDialogue();
        }
    }

    public void UnlockLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level1")
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1);
        }
        else if (currentScene == "Level2")
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1);
        }
    }

    private void UpdateUI(Indicator indicator)
    {
        scoreText.text = "Score: " + currentScore.ToString();
        comboText.text = "Combo: " + currentCombo.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();

        if (currentScore < milestones[0])
        {
            passengers.sprite = passengerState1;
        } else if (currentScore < milestones[1])
        {
            passengers.sprite = passengerState2;
        } else
        {
            passengers.sprite = passengerState3;
        }

        StartCoroutine(SpawnIndicator(indicator));
    }

    public void PerfectHit()
    {
        currentScore += (int)Mathf.Round(20 * currentMultiplier);
        currentCombo += 1;
        currentMultiplier = Math.Min(Math.Max(currentCombo / 10f, 1), 2);

        UpdateUI(Indicator.Perfect);
    }

    public void GoodHit()
    {
        currentScore += (int)Mathf.Round(10 * currentMultiplier);
        currentCombo += 1;
        currentMultiplier = Math.Min(Math.Max(currentCombo / 10f, 1), 2);

        UpdateUI(Indicator.Good);
    }

    public void NoteMissed()
    {
        currentCombo = 0;
        currentMultiplier = 1;
        
        UpdateUI(Indicator.Miss);
    }

    IEnumerator SpawnIndicator(Indicator indicator)
    {
        if (indicator == Indicator.Perfect)
        {
            perfectIndicator.gameObject.SetActive(true);
            goodIndicator.gameObject.SetActive(false);
            missIndicator.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            perfectIndicator.gameObject.SetActive(false);
        } else if (indicator == Indicator.Good)
        {
            perfectIndicator.gameObject.SetActive(false);
            goodIndicator.gameObject.SetActive(true);
            missIndicator.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            goodIndicator.gameObject.SetActive(false);
        } else
        {
            perfectIndicator.gameObject.SetActive(false);
            goodIndicator.gameObject.SetActive(false);
            missIndicator.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            missIndicator.gameObject.SetActive(false);
        }
    }


}

public enum Indicator
{
    Perfect,
    Good,
    Miss
}
