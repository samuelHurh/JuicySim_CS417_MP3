using UnityEngine;
using System.Collections;

public class CartProximityChecker : MonoBehaviour
{
    [SerializeField] GeneratorManager gmRef;
    [SerializeField] AccumulatorManager amRef;
    [SerializeField] CartManager cmRef;
    [SerializeField] GameObject mainCartRef;
    [SerializeField] float cartDetectDist;
    [SerializeField] float cartDetectionCooldown;

    public bool isAccumulator;

    private bool detectingCart;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detectingCart = false;
        StartCoroutine(PollDistance());
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    

    public IEnumerator PollDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if (Vector3.Distance(this.transform.position, mainCartRef.transform.position) < cartDetectDist && detectingCart == false)
            {
                detectingCart = true;
                if (isAccumulator)
                {
                    int amtFromCart = cmRef.DepositResources();
                    amRef.OnCartDetected(amtFromCart);
                } else
                {
                    gmRef.OnCartDetected();
                }
                
            } else
            {
                StartCoroutine(DetectionCooldown());
            }
        }
    }

    public IEnumerator DetectionCooldown()
    {
        yield return new WaitForSeconds(cartDetectionCooldown);
        detectingCart = false;
    }
}
