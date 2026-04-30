using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject PlayMenuPanel;
    public GameObject LevelsMenuPanel;

    private void Start()
    {
        ShowMainMenu();

    }

    public void ShowMainMenu()
    {
        MainMenuPanel.SetActive(true);
        PlayMenuPanel.SetActive(false);
        LevelsMenuPanel.SetActive(false);
    }

    public void ShowPlayMenu()
    {
        MainMenuPanel.SetActive(false);
        PlayMenuPanel.SetActive(true);
        LevelsMenuPanel.SetActive(false);
    }

    public void ShowLevelsMenu()
    {
        MainMenuPanel.SetActive(false);
        PlayMenuPanel.SetActive(false);
        LevelsMenuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        // run next scene
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
