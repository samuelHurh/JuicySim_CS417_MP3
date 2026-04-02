using System;
using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    [SerializeField] private int currResourceAmt;

    [SerializeField] private int maxResourceCapacity;
    [SerializeField] private TMPro.TextMeshProUGUI capacityText;
    [SerializeField] private TMPro.TextMeshProUGUI capacityIncreasedCostText;
    [SerializeField] private int capacityIncreasedAmount = 400;
    [SerializeField] private int capacityIncreasedCost = 200;
    [SerializeField] private TextMeshProUGUI myTextDisplay;

    [SerializeField] private GameObject SFX;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capacityText.text = maxResourceCapacity.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        UIManager.Instance.SetText("Ore", currResourceAmt.ToString());
    }

    public int TryAddResources(int amt)
    {
        int toAdd = Math.Min(maxResourceCapacity - currResourceAmt, amt);
        currResourceAmt += toAdd;
        myTextDisplay.text = currResourceAmt.ToString();
        int remainder = Math.Max(0, amt - toAdd);
        Debug.Log("Added " + toAdd + " resources to the cart. Current amount: " + currResourceAmt);
        SFX.SetActive(false);
        SFX.SetActive(true);
        return remainder;
    }
    public int DepositResources()
    {
        int toDropOff = currResourceAmt;
        currResourceAmt = 0;
        myTextDisplay.text = currResourceAmt.ToString();
        return toDropOff;
    }
    public int GetCurrentResources()
    {
        return currResourceAmt;
    }


    public void IncreaseCapacity()
    {
        if(currResourceAmt >= capacityIncreasedCost){
            maxResourceCapacity += capacityIncreasedAmount;
            currResourceAmt -= capacityIncreasedCost;
            capacityText.text = maxResourceCapacity.ToString();
            // Update required cost
            if (capacityIncreasedCost < 1600)
            {
                capacityIncreasedCost *= 2;
            }
            else
            {
                capacityIncreasedCost = 1600;
            }
                capacityIncreasedCostText.text = capacityIncreasedCost.ToString();
        }
        else
        {
            Debug.Log("broke");
        }
    }
}
