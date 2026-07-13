using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public void Start()
    {
        Instance = this;

        scoreText.text = "Score: 0";
        comboText.text = "Combo: 0";
        multiplierText.text = "Multiplier: x1";
        
        perfectIndicator.gameObject.SetActive(false);
        goodIndicator.gameObject.SetActive(false);
        missIndicator.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        noteSpawner.StartSpawner();
        rhythmConductor.StartMusic();
    }

    public void PerfectHit()
    {
        currentScore += (int)Mathf.Round(20 * currentMultiplier);
        currentCombo += 1;
        currentMultiplier = Math.Min(Math.Max(currentCombo / 10f, 1), 2);

        Debug.Log(currentMultiplier);

        scoreText.text = "Score: " + currentScore.ToString();
        comboText.text = "Combo: " + currentCombo.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();
        StartCoroutine(SpawnIndicator(Indicator.Perfect));
    }

    public void GoodHit()
    {
        currentScore += (int)Mathf.Round(10 * currentMultiplier);
        currentCombo += 1;
        currentMultiplier = Math.Min(Math.Max(currentCombo / 10f, 1), 2);

        scoreText.text = "Score: " + currentScore.ToString();
        comboText.text = "Combo: " + currentCombo.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();
        StartCoroutine(SpawnIndicator(Indicator.Good));
    }

    public void NoteMissed()
    {
        currentCombo = 0;
        currentMultiplier = 1;
        StartCoroutine(SpawnIndicator(Indicator.Miss));

        comboText.text = "Combo: " + currentCombo.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();
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
