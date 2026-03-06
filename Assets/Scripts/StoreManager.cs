using UnityEngine;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{

    [SerializeField] private CartManager cmRef;
    [SerializeField] private GameObject cartRef;

    [Header("Generator")]
    [SerializeField] private int generatorCost;
    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private Transform generatorSpawnPoint;

    [SerializeField] private TMPro.TextMeshProUGUI generatorCountText;
    private int generatorCount = 0;

    [Header("Refining Machine")]
    public List<GameObject> refiningMachines;
    private int numRefiningMachines = 0;
    [SerializeField] private TMPro.TextMeshProUGUI refiningMachineAmountText;
    [SerializeField] private int refiningMachineCost;
    [SerializeField] private TMPro.TextMeshProUGUI refiningMachineCostText;

    [Header("Refined Ore")]
    [SerializeField] private int refinedOreAmount = 0;
    [SerializeField] private TMPro.TextMeshProUGUI refinedOreAmountText;

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

            generatorCount += 100;
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
    }

    // This function convert Ore to Refining Machine (max num of Refining Machine = 4)
    public void BuyRefiningMachine()
    {
        if (refiningMachineCost > cmRef.GetCurrentResources() || numRefiningMachines >= 4)
        {
            Debug.Log("Can't Buy Refining Machine");
        }
        else
        {
            cmRef.TryAddResources(-refiningMachineCost);
            refiningMachines[numRefiningMachines].SetActive(true);
            numRefiningMachines++;

            // Update Amount
            if (refiningMachineAmountText != null)
            {
                refiningMachineAmountText.text = numRefiningMachines.ToString();
            }
            // Update Price Tag
            refiningMachineCost += 100;
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
}
