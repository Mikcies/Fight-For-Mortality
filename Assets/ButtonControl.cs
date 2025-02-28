using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
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

    public void Test() => Debug.Log("Test");
}
