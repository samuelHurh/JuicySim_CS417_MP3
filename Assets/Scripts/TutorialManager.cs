using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TutorialManager : MonoBehaviour
{
    public List<GameObject> tutorialPanels = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InitialCountdown());
        tutorialPanels[0].SetActive(true);
        tutorialPanels[1].SetActive(false);
        tutorialPanels[2].SetActive(false);
        tutorialPanels[3].SetActive(false);
        tutorialPanels[4].SetActive(false);
    }
    public IEnumerator InitialCountdown()
    {
        yield return new WaitForSeconds(8f);
        SpawnBuyTutorialPanels();
    }

    public void SpawnBuyTutorialPanels()
    {
        tutorialPanels[0].SetActive(false);
        tutorialPanels[1].SetActive(true);
        tutorialPanels[2].SetActive(true);
        tutorialPanels[3].SetActive(true);
    }

    public void SpawnEndTutorialPanel()
    {
        tutorialPanels[1].SetActive(false);
        tutorialPanels[2].SetActive(false);
        tutorialPanels[3].SetActive(false);
        tutorialPanels[4].SetActive(true);
    }
    
}
