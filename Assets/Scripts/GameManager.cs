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
    private int missCount = 0;

    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text multiplierText;

    public Image perfectIndicator;
    public Image goodIndicator;
    public Image missIndicator;

    private int currentCombo = 0;

    public NoteSpawner noteSpawner;
    public RhythmConductor rhythmConductor;

    // Passenger Stuff
    public Image passenger1;
    public Image passenger2;
    private int passenger1Index, passenger2Index;

    public List<PassengerSO> availablePassengers;

    public List<int> milestones = new List<int> { 1000, 2000 };

    [Header("Dialogue triggered at the end of a song based on the result (3 outcomes)")]
    public DialogueSO[] dialogues;

    [SerializeField] RectTransform jeepney;
    private Vector2 startPos;

    public float speed = 2f;
    public float height = 2f;

    public void Start()
    {
        Instance = this;

        scoreText.text = "Score: 0";
        comboText.text = "Combo: 0";
        multiplierText.text = "Multiplier: x1";

        perfectIndicator.gameObject.SetActive(false);
        goodIndicator.gameObject.SetActive(false);
        missIndicator.gameObject.SetActive(false);

        startPos = jeepney.anchoredPosition;

        // Select 2 passengers
        passenger1Index = UnityEngine.Random.Range(0, availablePassengers.Count);
        passenger2Index = UnityEngine.Random.Range(0, availablePassengers.Count);

        if (passenger2Index == passenger1Index)
        {
            passenger2Index = (passenger2Index + 1) % availablePassengers.Count;
        }

        // Check for static characters
        if ((availablePassengers[passenger1Index].isStatic && availablePassengers[passenger1Index].position != 0) || 
            (availablePassengers[passenger2Index].isStatic && availablePassengers[passenger2Index].position != 1))
        {
            int temp = passenger2Index;
            passenger2Index = passenger1Index;
            passenger1Index = temp;
            
        }

        passenger1.sprite = availablePassengers[passenger1Index].expressions[1];
        passenger2.sprite = availablePassengers[passenger2Index].expressions[1];

        StartCoroutine(Initialize());
    }

    private void Update()
    {
        float newY = startPos.y + (Mathf.Sin(Time.time * speed) * height);
        jeepney.anchoredPosition = new Vector2 (startPos.x, newY);
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

        // todo: support 2 passengers and randomize them

        if (currentScore < milestones[0] && missCount >= 10)
        {
            passenger1.sprite = availablePassengers[passenger1Index].expressions[2];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[2];
        }
        else if (currentScore < milestones[1])
        {
            passenger1.sprite = availablePassengers[passenger1Index].expressions[1];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[1];
        }
        else
        {
            passenger1.sprite = availablePassengers[passenger1Index].expressions[0];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[0];
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
        missCount += 1;
        
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
