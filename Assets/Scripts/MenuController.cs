using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator.enabled = false;
        StartCoroutine(ScreenFader.Instance.FadeIn());
    }
    public void PlayGame()
    {
        StartCoroutine(StartPlaying());   
    }

    IEnumerator StartPlaying()
    {
        // play animation
        animator.enabled = true;

        yield return new WaitForSeconds(2f);

        yield return (ScreenFader.Instance.FadeOut("ModeScreen"));
    }
}
