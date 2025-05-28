using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameReset.ResetAll();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Scene1");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // Only run this logic for the scene you care about
        if (scene.name == "Scene1")
        {
            // Reassign the follow target once everything is loaded
            if (CameraController.Instance != null)
            {
                CameraController.Instance.SetPlayerCameraFollow();
            }

            // Unsubscribe to avoid calling this again on other scenes
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
