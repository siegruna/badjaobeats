using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    private float startVolume;

    private void Start()
    {
        animator.enabled = false;
        StartCoroutine(ScreenFader.Instance.FadeIn());
        startVolume = audioSource.volume;
    }
    public void PlayGame()
    {
        StartCoroutine(StartPlaying());   
    }

    private IEnumerator AudioFadeOut(float fadeDuration)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        
    }

    IEnumerator StartPlaying()
    {
        // play animation
        animator.enabled = true;

        //yield return new WaitForSeconds(2f);
        yield return AudioFadeOut(2f);

        yield return (ScreenFader.Instance.FadeOut("ModeScreen"));
    }
}
