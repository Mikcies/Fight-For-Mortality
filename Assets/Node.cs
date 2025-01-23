using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; 

public class NodeManagment : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private TMP_Text interactionText;
    [SerializeField]
    private int nodeIndex;



    private bool isPlayerHere = false;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); 
        }
       
    }

    void Update()
    {
        if (isPlayerHere && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("CurrentNodeIndex", nodeIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneName);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = true;
            interactionText.text = $"Press 'E' to enter {sceneName}";
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

   
}
