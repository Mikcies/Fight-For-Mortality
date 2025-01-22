using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonControl : MonoBehaviour
{
    public void ReturnToMap()
    {
        SceneManager.LoadScene("WorldMap");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
