using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPcontroll : MonoBehaviour
{
    [SerializeField]
    int maxHealth = 3;
    [SerializeField]
    Animator Animator;
    internal int currentHealth;
    [SerializeField]
    GameObject loseCanvas;

    void Start()
    {

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void HandleDeath()
    {
        playerMove player = GetComponent<playerMove>();
        if (player != null)
        {
            player.enabled = false;
        }

        Animator.SetTrigger("Death");

        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        Time.timeScale = 0;
        loseCanvas.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "enemy" )
        {
            if (currentHealth > 0)
            {
                currentHealth--;
                Animator.Play("Hurt");
            }
            if(currentHealth == 0)
            {
                HandleDeath();
            }
        }

    }

}
