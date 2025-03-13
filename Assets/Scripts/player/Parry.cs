using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] float shieldDuration = 0.2f;
    [SerializeField] float cooldownDuration = 2.0f;
    [SerializeField] float punishDuration = 1.5f;
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform bulletSpawnPoint;

    internal bool isParrying = false;
    internal int Used;
    internal int punished;
    bool missedParry = false;
    float parryTimer = 0f;
    float cooldownTimer = 0f;
    private playerMove playerMovement;

    private List<string> selectedAbilities = new List<string>(); 

    void Start()
    {
        shield.SetActive(false);
        playerMovement = GetComponent<playerMove>();
        SetAbilities(AbilitySelection.GetSelectedAbilities()); 
        ApplyAbilities(); 
    }

    public void SetAbilities(List<string> abilities)
    {
        selectedAbilities = abilities;
    }

    void ApplyAbilities()
    {
        if (selectedAbilities.Contains("LongerParry"))
        {
            shieldDuration *= 1.5f; 
        }
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
        Used++;
    }

    private void DeactivateParry()
    {
        isParrying = false;
        shield.SetActive(false);
        playerMovement.canMove = true;
    }

    private void ApplyPunish()
    {
        playerMovement.Walkspeed = 2f;
        punished++;
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
