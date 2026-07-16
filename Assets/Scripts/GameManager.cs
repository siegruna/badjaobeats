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
    public Image passenger3;

    private int passenger1Index, passenger2Index, passenger3Index;

    public List<PassengerSO> availablePassengers;

    public List<int> milestones = new List<int> { 1000, 2000 };

    [Header("Dialogue triggered at the end of a song based on the result (3 outcomes)")]
    public DialogueSO[] dialogues;

    [SerializeField] RectTransform jeepney;
    private Vector2 startPos;

    public float speed = 2f;
    public float height = 2f;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip happySounds;
    [SerializeField] AudioClip angrySounds;

    bool angryFlag = false, happyFlag = false;

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

        // Initialize unselected passengers
        List<int> unselectedPassengers = new List<int>();
        for (int i = 0; i < availablePassengers.Count; i++)
        {
            unselectedPassengers.Add(i);
        }

        List<bool> occupiedSeats = new List<bool>();
        for (int i = 0; i < 3; i++)
        {
            occupiedSeats.Add(false);
        }

        // Check for key characters
        for (int i = 0; i < availablePassengers.Count; i++)
        {
            if (availablePassengers[i].isStatic)
            {
                int position = availablePassengers[i].position;
                if (position == 0)
                {
                    passenger1Index = i;
                }
                else if (position == 1)
                {
                    passenger2Index = i;
                }
                else
                {
                    passenger3Index = i;
                }

                occupiedSeats[position] = true;
                unselectedPassengers.Remove(i);
                
                break;
            }
        }

        for (int i = 0; i < occupiedSeats.Count; i++)
        {
            bool occupied = occupiedSeats[i];

            if (occupied)
            {
                continue;
            }

            int r = UnityEngine.Random.Range(0, unselectedPassengers.Count);

            if (i == 0)
            {
                passenger1Index = unselectedPassengers[r];

            }
            else if (i == 1)
            {
                passenger2Index = unselectedPassengers[r];
            }
            else
            {
                passenger3Index = unselectedPassengers[r];
            }

            unselectedPassengers.RemoveAt(r);
        }

        passenger1.sprite = availablePassengers[passenger1Index].expressions[1];
        passenger2.sprite = availablePassengers[passenger2Index].expressions[1];
        passenger3.sprite = availablePassengers[passenger3Index].expressions[1];

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
        }

        // Only start dialogue if in story mode.
        if (PlayerPrefs.GetInt("Mode", 0) == 0)
        {
            DialogueManager.Instance.StartDialogue();
        }
        else // Otherwise, just go back to the select screen
        {
            PlayerPrefs.SetString("MostRecent", SceneManager.GetActiveScene().name);
            StartCoroutine(ScreenFader.Instance.FadeOut("SelectScreen"));
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
            if (!angryFlag)
            {
                audioSource.PlayOneShot(angrySounds);
                angryFlag = true;
            }
            passenger1.sprite = availablePassengers[passenger1Index].expressions[2];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[2];
            passenger3.sprite = availablePassengers[passenger3Index].expressions[2];
        }
        else if (currentScore < milestones[1])
        {
            passenger1.sprite = availablePassengers[passenger1Index].expressions[1];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[1];
            passenger3.sprite = availablePassengers[passenger3Index].expressions[1];
        }
        else
        {
            if (!happyFlag)
            {
                audioSource.PlayOneShot(happySounds);
                happyFlag = true;   
            }
            passenger1.sprite = availablePassengers[passenger1Index].expressions[0];
            passenger2.sprite = availablePassengers[passenger2Index].expressions[0];
            passenger3.sprite = availablePassengers[passenger3Index].expressions[0];
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
        currentScore = Math.Max(currentScore - 20, 0);
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
