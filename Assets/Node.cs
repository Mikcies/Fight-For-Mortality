using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Pokud používáte TextMeshPro

public class NodeManagment : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private TMP_Text interactionText; 

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
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = true;
            HighlightNode(true);
            interactionText.text = $"Press 'E' to enter {sceneName}";
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = false;
            HighlightNode(false);

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    void HighlightNode(bool highlight)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = highlight ? Color.yellow : Color.white;
        }
        else
        {
            Debug.LogError("SpriteRenderer is missing.");
        }
    }
}
