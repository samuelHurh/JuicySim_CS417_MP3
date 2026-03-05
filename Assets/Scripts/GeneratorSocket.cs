using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Net.Sockets;
public class GeneratorSocket : MonoBehaviour
{
    [SerializeField] GameObject EmptyGeometry;
    [SerializeField] private XRSocketInteractor mySocket;
    [SerializeField] CartManager cmRef;
    [SerializeField] GameObject CartRef;

    private GeneratorManager currGenerator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void OccupySocket()
    {
        EmptyGeometry.SetActive(false);
        var insertedObject = mySocket.GetOldestInteractableSelected();
        var insertedGO = ((MonoBehaviour)insertedObject).gameObject;

        if (insertedGO.TryGetComponent<GeneratorManager>(out var gm))
        {
            gm.StartResourceGeneration(CartRef, cmRef);
            currGenerator = gm;
        }
        else
        {
            Debug.LogWarning($"Socketed object '{insertedGO.name}' has no GeneratorManager.");
        }
    }

    public void UnOccupySocket()
    {
        EmptyGeometry.SetActive(true);
        if (currGenerator == null)
        {
            Debug.Log("no generator is selected");   
            return;
        }
        currGenerator.HaltResourceGeneration(CartRef, cmRef);
        currGenerator = null;
    }
}
