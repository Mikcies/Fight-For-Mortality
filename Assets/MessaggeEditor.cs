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
    [SerializeField]
    TMP_Text time;
    [SerializeField]
    BossBase BossBase;
    void Start()
    {
        
    }

    
    void Update()
    {
        parry.text = "Sucessful Parry: " + parrycount.punished +"/" + parrycount.Used;
        HPleft.text = "HP left: " + playerHealth.currentHealth;
        time.text = "Duration of the fight: " + BossBase.finalTimeAlive;
    }
}
