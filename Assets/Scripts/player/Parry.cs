using System.Collections;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] float shieldDuration = 0.2f;
    [SerializeField] float cooldownDuration = 2.0f;
    [SerializeField] float punishDuration = 2f; 
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform bulletSpawnPoint;

    internal bool isParrying = false;
    bool missedParry = false; 
    float parryTimer = 0f;
    float cooldownTimer = 0f;
    private playerMove playerMovement;

    void Start()
    {
        shield.SetActive(false);
        playerMovement = GetComponent<playerMove>();
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F) && !isParrying && cooldownTimer <= 0)
        {
            ActivateParry();
        }

        if (isParrying)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer <= 0f)
            {
                if (missedParry)
                {
                    ApplyPunish();
                }
                DeactivateParry();
            }
        }
    }

    private void ActivateParry()
    {
        isParrying = true;
        missedParry = true; 
        shield.SetActive(true);
        playerMovement.canMove = false; 
        parryTimer = shieldDuration;
        cooldownTimer = cooldownDuration;
    }

    private void DeactivateParry()
    {
        isParrying = false;
        shield.SetActive(false);
        playerMovement.canMove = true;
    }

    private void ApplyPunish()
    {
        Debug.Log("Parry netrefeno! Aplikace penalizace.");
        playerMovement.Walkspeed = 2f; 
        Invoke("RestoreMovement", punishDuration); 
    }

    private void RestoreMovement()
    {
        playerMovement.Walkspeed = 5f; 
    }

    public void SuccessfulParry()
    {
        missedParry = false; 
    }
}
