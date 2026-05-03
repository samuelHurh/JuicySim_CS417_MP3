using System;
using UnityEngine;
using TMPro;
public class AccumulatorManager : MonoBehaviour
{

    private int myResourceCount;

    [SerializeField] private CartManager cmRef;
    [SerializeField] private TextMeshProUGUI myText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myResourceCount = 0;
        myText.text = myResourceCount.ToString();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void OnCartDetected(int toAdd)
    {
        myResourceCount += toAdd;
        myText.text = myResourceCount.ToString();
    }



}
