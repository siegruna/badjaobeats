using System.Collections;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class CutsceneSequence : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI displayText;
    public Image fadeOverlay; 

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public string nextSceneName = "Credits"; 

    private string sentence1 = "“Kaso pano naman kaya bukas.”";
    private string sentence2 = "“At sa susunod na linggo.”";
    private string sentence3 = "“Hanggang kelan kaya ito”";

    void Start()
    {
       
        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }
        displayText.text = "";

       
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        
        yield return StartCoroutine(TypeText(sentence1));

       
        yield return new WaitForSeconds(5f);

     
        displayText.text = "";
        yield return StartCoroutine(TypeText(sentence2));

        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(TypeText(sentence3));

        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(FadeToBlack());

    
        SceneManager.LoadScene(nextSceneName);
    }

 
    IEnumerator TypeText(string textToType)
    {
        displayText.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    IEnumerator FadeToBlack()
    {
        if (fadeOverlay == null)
        {
            Debug.LogError("FadeOverlay is missing from the Inspector! Cannot fade.");
            yield break; 
        }

        float fadeDuration = 1.5f; 
        float elapsedTime = 0f;
        Color color = fadeOverlay.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeOverlay.color = color;

            yield return null;
        }
    }
}