using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBoss : BossBase
{
    [Header("Teleport Settings")]
    [SerializeField] private List<Transform> teleportPoints;
    [SerializeField] private GameObject teleportIndicatorPrefab;
    [SerializeField] private float indicatorDuration = 1.5f;

    [Header("ShatterPlatform")]
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private float shatterDuration = 3f;

    [Header("Spore Settings")]
    [SerializeField] private GameObject sporePrefab;
    [SerializeField] private GameObject sporeIndicatorPrefab;
    [SerializeField] private float sporeLifetime = 3f;
    [SerializeField] private float indicatorDurationSpore = 1f;
    [SerializeField] private float sporeSpawnRadius = 2f;
    [SerializeField] private int sporeCount = 5;
    private List<GameObject> activeSpores = new List<GameObject>();

    [Header("Frenzied Teleport Settings")]
    [SerializeField] private int teleportCount = 5;
    [SerializeField] private float teleportDelay = 0.5f;
    [SerializeField] private int sporesPerTeleport = 2;

    [Header("Death")]
    [SerializeField]
    GameObject liverObject;
    [SerializeField]
    Transform liverSpawnPoint;

    [Header("Sound")]
    [SerializeField]
    AudioSource audioSource;
    void Start()
    {
        base.Start();
    }
    
    protected override void InitializeAttacks()
    {
        phase1Attacks.Add(ActivateSporeBurst);
        phase1Attacks.Add(Teleport);
        phase1Attacks.Add(ShatterPlatform);

        phase2Attacks.Add(Teleport);
        phase2Attacks.Add(ShatterPlatforms);
        phase2Attacks.Add(FrenziedTeleport);
    }
    protected override void HandleDeath()
    {
        activeSpores.Clear();
        isDead = true;
        currentState = BossState.Death;
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        finalTimeAlive = Mathf.FloorToInt(timeAlive);
        animator.SetTrigger("Death");
        collider.enabled = false;
        Instantiate(liverObject, liverSpawnPoint);
    }
    private void Teleport()
    {
        if (teleportPoints == null || teleportPoints.Count == 0)
        {
            Debug.LogWarning("Teleport points are not set!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, teleportPoints.Count);
        Transform targetPoint = teleportPoints[randomIndex];
        StartCoroutine(TeleportSequence(targetPoint));
    }

    private IEnumerator TeleportSequence(Transform targetPoint)
    {
        if (teleportIndicatorPrefab != null)
        {
            GameObject indicator = Instantiate(teleportIndicatorPrefab, targetPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(indicatorDuration);
            Destroy(indicator);
        }
        transform.position = targetPoint.position;
    }

    private void ShatterPlatform()
    {
        if (platforms == null || platforms.Count == 0)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, platforms.Count);
        GameObject selectedPlatform = platforms[randomIndex];
        StartCoroutine(IndicateShatter(selectedPlatform));
    }

    private IEnumerator IndicateShatter(GameObject platform)
    {
        Renderer platformRenderer = platform.GetComponentInChildren<Renderer>();
        Color originalColor = platformRenderer.material.color;
        Color warningColor = Color.red;

        float blinkDuration = 1f;
        int blinkTimes = 5;

        for (int i = 0; i < blinkTimes; i++)
        {
            platformRenderer.material.color = warningColor;
            yield return new WaitForSeconds(blinkDuration / (blinkTimes * 2));
            platformRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration / (blinkTimes * 2));
        }
        StartCoroutine(RestorePlatform(platform));
    }

    private IEnumerator RestorePlatform(GameObject platform)
    {
        platform.SetActive(false);
        yield return new WaitForSeconds(shatterDuration);
        platform.SetActive(true);
    }

    public void ActivateSporeBurst()
    {
        StartCoroutine(SpawnSpores());
    }

    private IEnumerator SpawnSpores()
    {
        for (int i = 0; i < sporeCount; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * sporeSpawnRadius;
            GameObject indicator = Instantiate(sporeIndicatorPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(indicatorDurationSpore);
            Destroy(indicator);

            GameObject spore = Instantiate(sporePrefab, spawnPos, Quaternion.identity);
            activeSpores.Add(spore);
        }

        yield return new WaitForSeconds(sporeLifetime);
        foreach (GameObject spore in activeSpores)
        {
            if (spore != null)
                Destroy(spore);
        }
        activeSpores.Clear();
    }

    public void FrenziedTeleport()
    {
        animator.SetBool("Teleport", true);
        StartCoroutine(FrenziedTeleportSequence());
        
    }

    private IEnumerator FrenziedTeleportSequence()
    {
        sporeLifetime = 1f;
        for (int i = 0; i < teleportCount; i++)
        {
            if (teleportPoints == null || teleportPoints.Count == 0)
            {
                Debug.LogWarning("Teleport points are not set!");
                yield break;
            }

            int randomIndex = UnityEngine.Random.Range(0, teleportPoints.Count);
            Transform targetPoint = teleportPoints[randomIndex];

            GameObject teleportIndicator = Instantiate(teleportIndicatorPrefab, targetPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(indicatorDuration);
            Destroy(teleportIndicator);
            transform.position = targetPoint.position;

            for (int j = 0; j < sporesPerTeleport; j++)
            {
                Vector3 spawnPos = targetPoint.position + (Vector3)UnityEngine.Random.insideUnitCircle * sporeSpawnRadius;
                GameObject sporeIndicator = Instantiate(sporeIndicatorPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(indicatorDurationSpore);
                Destroy(sporeIndicator);

                GameObject spore = Instantiate(sporePrefab, spawnPos, Quaternion.identity);
                activeSpores.Add(spore);
            }
            yield return new WaitForSeconds(teleportDelay);
        }
        StartCoroutine(CleanupSpores(sporeLifetime));
    }

    private IEnumerator CleanupSpores(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject spore in activeSpores)
        {
            if (spore != null)
                Destroy(spore);
        }
        activeSpores.Clear();
    }


    private void ShatterPlatforms()
    {
        animator.SetBool("Stomp", true);
        if (platforms == null || platforms.Count == 0)
        {
            return;
        }

        int platformsToBreak = Mathf.Min(2, platforms.Count); 
        List<GameObject> selectedPlatforms = new List<GameObject>();

        while (selectedPlatforms.Count < platformsToBreak)
        {
            int randomIndex = UnityEngine.Random.Range(0, platforms.Count);
            GameObject selectedPlatform = platforms[randomIndex];

            if (!selectedPlatforms.Contains(selectedPlatform))
            {
                selectedPlatforms.Add(selectedPlatform);
            }
        }

        foreach (GameObject platform in selectedPlatforms)
        {
            StartCoroutine(IndicateShatters(platform));
        }
    }

    private IEnumerator IndicateShatters(GameObject platform)
    {
        animator.SetBool("Stomp", false);
        shatterDuration = 3f;
        Renderer platformRenderer = platform.GetComponentInChildren<Renderer>();
        Color originalColor = platformRenderer.material.color;
        Color warningColor = Color.red;

        float blinkDuration = 1f;
        int blinkTimes = 5;

        for (int i = 0; i < blinkTimes; i++)
        {
            platformRenderer.material.color = warningColor;
            yield return new WaitForSeconds(blinkDuration / (blinkTimes * 2));
            platformRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration / (blinkTimes * 2));
        }
        StartCoroutine(RestorePlatforms(platform));
    }

    private IEnumerator RestorePlatforms(GameObject platform)
    {
        platform.SetActive(false);
        yield return new WaitForSeconds(shatterDuration);
        platform.SetActive(true);
    }
}