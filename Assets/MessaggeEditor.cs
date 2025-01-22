using UnityEngine;
using TMPro;


public class MessaggeEditor : MonoBehaviour
{
    [SerializeField]
    TMP_Text parry;
    [SerializeField]
    TMP_Text HPleft;
    [SerializeField]
    Parry parrycount;
    [SerializeField]
    HPcontroll playerHealth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        parry.text = "Sucessful Parry: " + parrycount.punished +"/" + parrycount.Used;
        HPleft.text = "HP left: " + playerHealth.currentHealth;
    }
}
