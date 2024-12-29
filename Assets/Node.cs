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
    private Image image;

    
    private bool isPlayerHere = false;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); 
        }
        image.enabled = false;
    }

    void Update()
    {
        if (isPlayerHere && Input.GetKeyDown(KeyCode.E))
        {
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = true;
            image.enabled = true;
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
            image.enabled = false;
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
