using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SurpriseSurge : MonoBehaviour
{
    private GeneratorManager[] generators;

    [Header("Surge Settings")]
    [SerializeField] private float surgeRate = 0.25f;
    [SerializeField] private float surgeDuration = 5f;

    [Header("Random Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 15f;
    [SerializeField] private float maxSpawnDelay = 30f;

    [Header("UI")]
    [SerializeField] private GameObject blinkWindow;
    [SerializeField] private Image targetImage;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private float visibleDuration = 3f;
    [SerializeField] private float originalAlpha = 0.8f;

    private bool blinking = false;
    private bool surgeVisible = false;
    private Coroutine blinkRoutine;

    void Start()
    {
        blinkWindow.SetActive(false);
        StartCoroutine(RandomSurgeLoop());
    }

    IEnumerator RandomSurgeLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(ShowSurgeWindow());
        }
    }

    IEnumerator ShowSurgeWindow()
    {
        surgeVisible = true;
        StartBlink();

        float timer = 0f;
        while (timer < visibleDuration && surgeVisible)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (surgeVisible)
        {
            StopBlink();
            surgeVisible = false;
            Debug.Log("Surprise Surge expired.");
        }
    }

    public void OnSurgeClicked()
    {
        if (!surgeVisible) return;

        surgeVisible = false;
        StopBlink();
        TriggerSurge();
    }

    public void TriggerSurge()
    {
        StartCoroutine(SurgeCoroutine());
    }

    IEnumerator SurgeCoroutine()
    {
        FindAllGenerators();

        Debug.Log("Surprise Surge started!");

        foreach (GeneratorManager g in generators)
        {
            g.IncreaseResourceRate(surgeRate);
        }

        yield return new WaitForSeconds(surgeDuration);

        foreach (GeneratorManager g in generators)
        {
            g.IncreaseResourceRate(1f/surgeRate);
        }

        Debug.Log("Surprise Surge ended!");
    }

    void FindAllGenerators()
    {
        generators = FindObjectsOfType<GeneratorManager>();
        Debug.Log("Found " + generators.Length + " generators in scene.");
    }

    public void StartBlink()
    {
        blinkWindow.SetActive(true);

        if (blinking) return;

        blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        blinking = true;
        bool isRed = false;

        while (true)
        {
            Color c = isRed ? Color.red : Color.white;
            c.a = originalAlpha;
            targetImage.color = c;

            isRed = !isRed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void StopBlink()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        Color c = Color.white;
        c.a = originalAlpha;
        targetImage.color = c;

        blinking = false;
        blinkWindow.SetActive(false);
    }
}