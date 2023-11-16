using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{


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
}
