using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class StoreManager : MonoBehaviour
{

    [SerializeField] private CartManager cmRef;
    [SerializeField] private GameObject cartRef;

    [Header("Generator")]
    [SerializeField] private int generatorCost;
    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private Transform generatorSpawnPoint;

    [SerializeField] private TMPro.TextMeshProUGUI generatorCountText;
    [SerializeField] private TMPro.TextMeshProUGUI generatorCostText;
    private int generatorCount = 0;

    [Header("Refining Machine")]
    public List<GameObject> refiningMachines;
    private int numRefiningMachines = 0;
    [SerializeField] private TMPro.TextMeshProUGUI refiningMachineAmountText;
    [SerializeField] private int refiningMachineCost = 100;
    [SerializeField] private TMPro.TextMeshProUGUI refiningMachineCostText;
    [SerializeField] private GameObject refiningMachineCanvas;
    [SerializeField] private GameObject refiningMachineCanvas_lock;

    [Header("Refined Ore")]
    private bool isRefinedOreUnlocked = false;
    public int refinedOreAmount = 0;
    public float resourceRate;
    private float resourceTimer;
    public int resourceIncrementSize;
    [SerializeField] private TMPro.TextMeshProUGUI refinedOreAmountText;
    [SerializeField] private GameObject refindedOreCanvas;
    [SerializeField] private GameObject refindedOreCanvas_lock; 
    [SerializeField] private CartMovement cartMove; 

    [Header("Cart Speed Boost")]
    [SerializeField] private int speedBoostOreCost = 50;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float speedBoostDuration = 30f;

    [SerializeField] private XRBaseInputInteractor left_controller;
    [SerializeField] private XRBaseInputInteractor  right_controller;
 
    private bool generatorInUpgrader;
    private GeneratorManager generatorToUpgrade;
    private float upgradeCost = 200f;
    [SerializeField] XRSocketInteractor upgradeSocket;

    public void BuyCartSpeedBoost()
    {
        if (cmRef.GetCurrentResources() < speedBoostOreCost)
        {
            Debug.Log("Not enough normal ore for speed boost.");
            return;
        }
        cmRef.TryAddResources(-speedBoostOreCost); 
        if (cartMove != null)
        {
            cartMove.IncreaseSpeedTemporarily(speedBoostDuration);
            left_controller.SendHapticImpulse(1f, speedBoostDuration);
            right_controller.SendHapticImpulse(1f, speedBoostDuration);
            Debug.Log($"Cart speed boosted for {speedBoostDuration} seconds.");
        }
    }

    public void GeneratorPlacedInUpgrader()
    {
        generatorInUpgrader = true;
        var insertedObject = upgradeSocket.GetOldestInteractableSelected();
        var insertedGO = ((MonoBehaviour)insertedObject).gameObject;

        if (insertedGO.TryGetComponent<GeneratorManager>(out var gm))
        {
            generatorToUpgrade = gm;
        }
        else
        {
            Debug.LogWarning($"Socketed object '{insertedGO.name}' has no GeneratorManager.");
        }
    }

    public void GeneratorRemovedFromUpgrader()
    {
        generatorInUpgrader = false;
    }

    public void TryUpgradeGenerator()
    {
        // Detect if generator is placed
        if (!generatorInUpgrader)
        {
            Debug.Log("No generator to upgrader");
            return;
        }
        // Check if adequate ore
        if (cmRef.GetCurrentResources() >= upgradeCost)
        {
            cmRef.TryAddResources(-(int)upgradeCost);
            generatorToUpgrade.UpgradeGenerator();
        } else
        {
            Debug.Log("Insufficient upgrade materials");
        }
        
        // Call Generator for isupgraded check, change material, upgrade rate

    }


    public void buyGenerator()
    {
        if(generatorCost > cmRef.GetCurrentResources())
        {
            Debug.Log("Can't Buy");
        }
        else
        {
            cmRef.TryAddResources(-generatorCost); 
            GameObject spawnedGenerator = Instantiate(generatorPrefab, generatorSpawnPoint.position, 
                        generatorSpawnPoint.rotation);
            spawnedGenerator.GetComponent<GeneratorManager>().SetCartAndRef(cartRef, cmRef);
            Debug.Log("Cart Purchased");

            generatorCost *= 2;
            if (generatorCostText != null)
            {
                generatorCostText.text = generatorCost.ToString();
            }
            generatorCount += 1;
            if (generatorCountText != null)
            {
                generatorCountText.text = generatorCount.ToString();
            }
        }
    }
    private void Start()
    {
        if (generatorCountText != null)
        {
            generatorCountText.text = generatorCount.ToString();
        }

        // Lock refinied ore UI
        refiningMachineCanvas.SetActive(false);
        refiningMachineCanvas_lock.SetActive(true);
        refindedOreCanvas.SetActive(false);
        refindedOreCanvas_lock.SetActive(true);
    }

    void Update()
    {
        // Use Euler integrated over timesteps to increase refined ore amount
        if (isRefinedOreUnlocked)
        {
            resourceTimer += Time.deltaTime;

            while (resourceTimer >= resourceRate)
            {
                resourceTimer -= resourceRate;

                refinedOreAmount += resourceIncrementSize;
                refinedOreAmountText.text = refinedOreAmount.ToString();
            }
        }

    }

    // This function purchase Refining Machine using Refined Ore (max num of Refining Machine = 4)
    public void BuyRefiningMachine()
    {
        if (refiningMachineCost > refinedOreAmount || numRefiningMachines >= 4)
        {
            Debug.Log("Can't Buy Refining Machine");
        }
        else
        {
            refinedOreAmount -= refiningMachineCost;
            refiningMachines[numRefiningMachines].SetActive(true);
            numRefiningMachines++;

            // Update Amount
            if (refiningMachineAmountText != null)
            {
                refiningMachineAmountText.text = numRefiningMachines.ToString();
            }
            // Update Price Tag
            refiningMachineCost *= 2;
            if (refiningMachineCostText != null)
            {
                refiningMachineCostText.text = refiningMachineCost.ToString();
            }
            Debug.Log("1 Refining Machine Purchased");
        }   
    }

    // Update refined ore amount when player click the machine
    public void CollectRefinedOre(int amount)
    {
        // Check Timer
        refinedOreAmount += amount;
        if (refinedOreAmountText != null)
        {
            refinedOreAmountText.text = refinedOreAmount.ToString();
        }
    }

    // Unlockable UI: Use Ore
    public void UnlockUI()
    {
        if (800 > cmRef.GetCurrentResources())
        {
            Debug.Log("Can't unlock UI");
        }
        else
        {
            isRefinedOreUnlocked = true;
            cmRef.TryAddResources(-800);
            // Show Refining Machine Option
            refiningMachineCanvas.SetActive(true);
            refiningMachineCanvas_lock.SetActive(false);
            refindedOreCanvas.SetActive(true);
            refindedOreCanvas_lock.SetActive(false);

        }
    }


}
