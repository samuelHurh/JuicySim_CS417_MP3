using UnityEngine;
using System.Collections;
using TMPro;

public class GeneratorManager : MonoBehaviour
{
    public float resourceRate;

    private int myResourceCount;

    public int resourceIncrementSize;

    [SerializeField] private CartManager cmRef;
    [SerializeField] private TextMeshProUGUI myText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myResourceCount = 0;
        StartCoroutine(AutoIncrementResources());
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void OnCartDetected()
    {
        myResourceCount = cmRef.TryAddResources(myResourceCount);
        myText.text = myResourceCount.ToString();
    }



    public IEnumerator AutoIncrementResources()
    {
        while(true)
        {
            yield return new WaitForSeconds(resourceRate);
            myResourceCount += resourceIncrementSize;
            myText.text = myResourceCount.ToString();
        }
        

    }
}
