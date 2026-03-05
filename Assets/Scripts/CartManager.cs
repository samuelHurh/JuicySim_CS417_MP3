using System;
using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    [SerializeField] private int currResourceAmt;

    [SerializeField] private int maxResourceCapacity;
    [SerializeField] private TextMeshProUGUI myTextDisplay;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        return Math.Max(0, amt - toAdd);
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
}
