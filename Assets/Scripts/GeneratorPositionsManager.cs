using UnityEngine;

public class GeneratorPositionsManager : MonoBehaviour
{
    private bool firstGeneratorPlaced;
    public TutorialManager tmRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstGeneratorPlaced = false;
    }


    public void FirstGeneratorWasPlaced()
    {
        if (firstGeneratorPlaced == true) return;
        firstGeneratorPlaced = true;
        tmRef.SpawnEndTutorialPanel();
    }
}
