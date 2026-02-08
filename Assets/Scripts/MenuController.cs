using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    public int gameSceneIndex = 0;
    public void StartGame()
    {
        // Log to console for debugging during the assignment
        Debug.Log("Starting Game...");

        // Load the scene by name
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

        // This works in a standalone build (.exe, .app)
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}