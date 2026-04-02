using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Production Settings")]
    [SerializeField] private float productionTime = 5f;
    [SerializeField] private int rewardAmount = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("References")]
    [SerializeField] private StoreManager storeManager;
    [SerializeField] private RefinedOreMachineFeedback myFeedback;
    [SerializeField] private ParticleSystem expCostPS;

    private float timer;
    private bool isReady = false;

    void Start()
    {
        timer = productionTime;
        UpdateUI();
    }

    void Update()
    {
        if (isReady) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            isReady = true;
            UpdateUI();
        }
        else
        {
            UpdateUI();
        }
    }

    public void CollectResource()
    {
        if (!isReady)
        {
            Debug.Log("Resource is not ready yet.");
            return;
        }

        if (storeManager != null)
        {
            storeManager.CollectRefinedOre(rewardAmount);
            myFeedback.FireParticles();
        }

        Debug.Log("Collected " + rewardAmount + " resource.");

        isReady = false;
        timer = productionTime;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (statusText == null) return;

        if (isReady)
        {
            statusText.text = "Collect";
        }
        else
        {
            statusText.text = "Ready in: " + Mathf.Ceil(timer).ToString() + "s";
        }
    }
}