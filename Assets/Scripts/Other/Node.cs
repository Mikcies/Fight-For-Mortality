using UnityEngine;
using TMPro;

public class NodeManagment : MonoBehaviour
{
    [SerializeField] internal string sceneName; 
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private int nodeIndex;
    [SerializeField] private GameObject abilitySelection;
    [SerializeField] private TMP_Text bossName;
    [SerializeField] private TMP_Text bossLore;
    [SerializeField] private string bossNameString;
    [SerializeField] private string bossLoreString;
    [SerializeField] private ButtonControl button; 
    [SerializeField] NodeMovementManager nodeMovementManager;

    private bool isPlayerHere = false;

    void Start()
    {
        abilitySelection.gameObject.SetActive(false);
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (button == null)
        {
            button = abilitySelection.GetComponentInChildren<ButtonControl>();
        }
    }

    void Update()
    {
        if (isPlayerHere && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("CurrentNodeIndex", nodeIndex);
            bossName.text = bossNameString;
            bossLore.text = bossLoreString;
            PlayerPrefs.Save();

            if (button != null)
            {
                button.SetScene(sceneName);
            }
            else
            {
                Debug.LogError("ButtonControl not found!");
            }

            abilitySelection.gameObject.SetActive(!abilitySelection.gameObject.activeSelf);
            if(abilitySelection.gameObject.activeSelf)
            {
                nodeMovementManager.enabled = false;
            }
            else
            {
                nodeMovementManager.enabled = true;
            }
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

    public void ChangeScene(string newScene)
    {
        sceneName = newScene;
        if (button != null)
        {
            button.SetScene(sceneName); 
        }
    }
}
