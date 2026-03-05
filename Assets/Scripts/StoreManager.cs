using UnityEngine;

public class StoreManager : MonoBehaviour
{

    [SerializeField] private CartManager cmRef;
    [SerializeField] private GameObject cartRef;
    [SerializeField] private int generatorCost;
    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private Transform generatorSpawnPoint;


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
        }
    }
    
}
