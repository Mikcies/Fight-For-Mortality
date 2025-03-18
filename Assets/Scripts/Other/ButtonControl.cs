using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    private string scene;

    public void SetScene(string sceneName)
    {
        scene = sceneName;
        Debug.Log($"Scene set to: {scene}");
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("WorldMap");
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(scene))
        {
            Debug.Log($"Loading scene: {scene}");
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Scene name is empty!");
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
