using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;         

public class PanAndStop : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;

    public float targetXPosition = -15.5f;

    [Header("Scene Transition")]
    public float delayTime = 5f;

   
    public string menuSceneName = "MainMenu";

   
    private bool hasReachedTarget = false;

    private void Awake()
    {
        StartCoroutine(ScreenFader.Instance.FadeIn());
    }

    void Update()
    {
        if (!hasReachedTarget)
        {
           
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

 
            if (transform.position.x <= targetXPosition)
            {
              
                transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);

            
                hasReachedTarget = true;

          
                StartCoroutine(WaitAndLoadMenu());
            }
        }
    }

 
    IEnumerator WaitAndLoadMenu()
    {
        Debug.Log("Background stopped. Counting down " + delayTime + " seconds...");

       
        yield return new WaitForSeconds(delayTime);

        Debug.Log("Loading Scene: " + menuSceneName);


        yield return ScreenFader.Instance.FadeOut(menuSceneName);
    }
}