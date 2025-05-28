using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    public static GameOverUIController Instance;

    public CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private Image blackFadeImage;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance assigned to: " + Instance);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("Duplicate GameOverUIController detected. This: " + this + ", Existing Instance: " + Instance);
            return;
        }

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (blackFadeImage == null)
            Debug.LogWarning("blackFadeImage is NOT assigned in inspector!");

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }




    public void TriggerGameOver()
    {
        Debug.Log("TriggerGameOver called. Instance is: " + Instance);

        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }


    private void OnDestroy()
    {
        Debug.LogWarning(" GameOverUIController was destroyed!");
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;

        // Ensure everything is visible
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        blackFadeImage.color = new Color(0, 0, 0, 0);

        while (t < fadeDuration)
        {
            float normalizedTime = t / fadeDuration;

            // Fade canvas
            canvasGroup.alpha = normalizedTime;

            // Fade background
            blackFadeImage.color = new Color(0, 0, 0, normalizedTime);

            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        blackFadeImage.color = Color.black;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Wait for player input, or show buttons to restart manually
    }
    public void RestartGame()
    {
     GameReset.ResetAll();

      // Hook into scene loaded event
      SceneManager.sceneLoaded += OnSceneLoaded;

     SceneManager.LoadScene("Scene1");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log("Scene loaded: " + scene.name);

    // Reassign camera to follow new player
    CameraController.Instance.SetPlayerCameraFollow();

    // Unsubscribe so it doesn't run again every scene load
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

}
