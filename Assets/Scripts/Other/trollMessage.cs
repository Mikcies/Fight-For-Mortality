using TMPro;
using UnityEngine;

public class TrollMessage : MonoBehaviour
{
    [SerializeField]
    TMP_Text Text;
    private string[] messages = {
        "You shall not pass!",
        "Trolling is an art.",
        "Why so serious?"
    };

    void Start()
    {
        ShowRandomMessage();
    }

    void ShowRandomMessage()
    {
        int randomIndex = Random.Range(0, messages.Length);
        Text.text = messages[randomIndex];

    }
}
