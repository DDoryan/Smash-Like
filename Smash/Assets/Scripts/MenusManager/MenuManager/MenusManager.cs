using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject gameOver;
    public EventSystem pauseEventSystem;
    public EventSystem controlsEventSystem;
    public EventSystem gameOverEventSystem;

    public void Start()
    {
        Instance = this;
    }

    public void LoadIndexScene(int _indexScene)
    {
        SceneManager.LoadScene(_indexScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackInMainMenu(int _indexScene)
    {
        SceneManager.LoadScene(_indexScene);
    }

    public void BackInPause(int _indexScene)
    {
        SceneManager.LoadScene(_indexScene);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
        controlsEventSystem.gameObject.SetActive(false);
        pauseEventSystem.gameObject .SetActive(true);
    }

    public void EndPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ControlMenu()
    {
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
        pauseEventSystem.gameObject.SetActive(false);
        controlsEventSystem.gameObject.SetActive(true);
    }
    public void DisableGameOver()
    {
        gameOverEventSystem.gameObject.SetActive(false);
        pauseEventSystem.gameObject.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1;
    }

    public void EnableGameOver()
    {
        pauseEventSystem.gameObject.SetActive(false);
        gameOverEventSystem.gameObject.SetActive(true);
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
}