using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBoss : BossBase
{
    private System.Random random = new System.Random();

    [Header("Teleport Settings")]
    [SerializeField] private List<Transform> teleportPoints;
    [SerializeField] private GameObject teleportIndicatorPrefab;
    [SerializeField] private float indicatorDuration = 1.5f;

    [Header("ShatterPlatform")]
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private float shatterDuration = 3f;

    void Start()
    {
        base.Start();
    }



    protected override void InitializeAttacks()
    {
        phase1Attacks.Add(Teleport);
        phase1Attacks.Add(ShatterPlatform);
    }

    private void Teleport()
    {
        animator.SetBool("Teleport", true);
        if (teleportPoints == null || teleportPoints.Count == 0)
        {
            Debug.LogWarning("Teleport points are not set!");
            return;
        }

        int randomIndex = random.Next(teleportPoints.Count);
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
        animator.SetBool("Teleport", false);
        transform.position = targetPoint.position;
    }

    private void ShatterPlatform()
    {
        animator.SetBool("Stomp", true);
        if (platforms == null || platforms.Count == 0)
        {
            return;
        }

        int randomIndex = random.Next(platforms.Count);
        GameObject selectedPlatform = platforms[randomIndex];

        StartCoroutine(IndicateShatter(selectedPlatform));
    }

    private IEnumerator IndicateShatter(GameObject platform)
    {
        if (platform != null)
        {
            Renderer platformRenderer = platform.GetComponent<Renderer>();
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
            animator.SetBool("Stomp", false);
            StartCoroutine(RestorePlatform(platform));
        }
    }

    private IEnumerator RestorePlatform(GameObject platform)
    {
        if (platform != null)
        {
            platform.SetActive(false);
            Debug.Log("Platform shattered: " + platform.name);

            yield return new WaitForSeconds(shatterDuration);

            platform.SetActive(true);
            Debug.Log("Platform restored: " + platform.name);
        }
    }

}
