using UnityEngine;

public class OpenWin : MonoBehaviour
{
    [SerializeField]
     GameObject winCanvas;
    // Update is called once per frame
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            winCanvas.SetActive(true);
        }
    }
}
