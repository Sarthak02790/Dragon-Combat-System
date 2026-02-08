using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int mainMenuIndex = 0;
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public GameObject attackButtonsPanel;
    public GameObject fightTextPanel;

    void Start()
    {
        StartCoroutine(ShowFightText());
        winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnDragonDeath(string deadDragonName)
    {
        attackButtonsPanel.SetActive(false);
        winPanel.SetActive(true);
        SoundManager.instance.TriggerGameOver();
        winText.text = deadDragonName == "AI" ? "PLAYER WINS!" : "AI WINS!";
        Time.timeScale = 0.5f;
    }

    IEnumerator ShowFightText()
    {
        // Ensure the panel is visible
        if (fightTextPanel != null)
        {
            fightTextPanel.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            // Deactivate the panel
            fightTextPanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {

        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene(mainMenuIndex);
    }
}