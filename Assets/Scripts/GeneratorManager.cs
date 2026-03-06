using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GeneratorManager : MonoBehaviour
{
    public float resourceRate;

    private int myResourceCount;

    public int resourceIncrementSize;

    [SerializeField] private CartManager cmRef;
    [SerializeField] private GameObject cartRef;
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private CartProximityChecker cpcRef;

    [SerializeField] private Material upgradedMaterial;
    
    private float resourceTimer;
    private bool generatorEffective;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myResourceCount = 0;
        generatorEffective = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!generatorEffective) return;
        //Euler integration stuff if they care to look in the code
        resourceTimer += Time.deltaTime;

        while (resourceTimer >= resourceRate)
        {
            resourceTimer -= resourceRate;

            myResourceCount += resourceIncrementSize;
            myText.text = myResourceCount.ToString();
        }
        
    }

    public void OnCartDetected()
    {
        if (!generatorEffective) return;

        myResourceCount = cmRef.TryAddResources(myResourceCount);
        myText.text = myResourceCount.ToString();
        
    }

    public void SetCartAndRef(GameObject mainCartRef, CartManager cm)
    {
        cmRef = cm;
        cartRef = mainCartRef;
    }



    public void StartResourceGeneration(GameObject cartRef, CartManager cm)
    {
        generatorEffective = true;
        cpcRef.StartPolling(cartRef,cmRef, false);

    }
    public void HaltResourceGeneration(GameObject cartRef, CartManager cm)
    {
        generatorEffective = false;
        cpcRef.StopPolling();
    }

    public float GetResourceRate()
    {
        return resourceRate;
    }

    public void IncreaseResourceRate(float surgeRate)
    {
        resourceRate *= surgeRate;
    }

    public void UpgradeGenerator()
    {
        this.transform.gameObject.GetComponent<Renderer>().material = upgradedMaterial;
        resourceRate *= 0.5f;
    }
}
