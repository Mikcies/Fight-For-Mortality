using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    Animator anim;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        anim.speed = 0;
        anim.enabled = false;
        PauseMenu.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        anim.enabled = true;
        PauseMenu.SetActive(false);
        isPaused = false;
    }
}
