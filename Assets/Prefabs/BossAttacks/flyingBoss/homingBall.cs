using System.Collections;
using UnityEngine;

public class homingBall : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float homingDuration = 3f; 
    private Transform target;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not set!");
        }
        StartCoroutine(SelfDestructAfterDuration());
    }

    public void InitializeTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private IEnumerator SelfDestructAfterDuration()
    {
        yield return new WaitForSeconds(homingDuration);

        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}