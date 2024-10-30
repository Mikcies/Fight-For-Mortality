using System.Collections;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] float shieldDuration = 0.2f;
    [SerializeField] float cooldownDuration = 2.0f;
    [SerializeField] float punishDuration = 1.5f; // Penalizace při netrefení parry
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform bulletSpawnPoint;

    internal bool isParrying = false;
    bool missedParry = false; // Označuje, že parry byla netrefena
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

            // Po vypršení času na parry kontrolujeme, zda byl parry úspěšný
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
        missedParry = true; // Předpokládáme, že parry mine
        shield.SetActive(true);
        playerMovement.canMove = false; // Zastaví pohyb během parry
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
        Invoke("RestoreMovement", punishDuration); // Obnovení pohybu po penalizaci
    }

    private void RestoreMovement()
    {
        playerMovement.Walkspeed = 5f; // Nastaví původní rychlost hráče
    }

    // Metoda pro úspěšný parry - volá se z ShieldParry při kolizi s projektily
    public void SuccessfulParry()
    {
        missedParry = false; // Zruší penalizaci při úspěšném parry
    }
}
