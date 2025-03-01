using System.Collections;
using UnityEngine;

public class vine : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public void Activate(float moveSpeed)
    {
        speed = moveSpeed;
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        while (true)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }
    }
}