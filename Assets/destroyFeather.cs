using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyFeather : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
