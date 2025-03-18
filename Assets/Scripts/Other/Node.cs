using UnityEngine;
using TMPro;

public class NodeManagment : MonoBehaviour
{
    [SerializeField] internal string sceneName; 
    [SerializeField] private int nodeIndex;
    [SerializeField] private GameObject abilitySelection;
    [SerializeField] private TMP_Text bossName;
    [SerializeField] private TMP_Text bossLore;
    [SerializeField] private string bossNameString;
    [SerializeField] private string bossLoreString;
    private ButtonControl button; 
    [SerializeField] NodeMovementManager nodeMovementManager;
    [SerializeField] GameObject legend;

    private bool isPlayerHere = false;

    void Start()
    {
        abilitySelection.gameObject.SetActive(false);
        

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
                legend.SetActive(false);
                nodeMovementManager.enabled = false;
            }
            else
            {
                legend.SetActive(true);
                nodeMovementManager.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerHere = false;
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
