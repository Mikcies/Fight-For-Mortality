using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpshow : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    [SerializeField]
    HPcontroll hp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "HP: " + hp.currentHealth.ToString();
    }
}
