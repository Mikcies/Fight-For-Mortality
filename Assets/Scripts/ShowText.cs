using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] string messageToShow;
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == collision.CompareTag("Player"))
        {
            canvas.enabled = true;
            text.text = messageToShow;
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == collision.CompareTag("Player"))
        {
            canvas.enabled = false;
            text.text = messageToShow;
        }
    }
}
