using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField]
    private List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>();
 
    [SerializeField]
    private List<string> labels = new List<string>();

    private Dictionary<string, TextMeshProUGUI> labelToElement = new Dictionary<string, TextMeshProUGUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        labelToElement.Clear();
        for (int i = 0; i < textElements.Count && i < labels.Count; i++)
        {
            if (!string.IsNullOrEmpty(labels[i]) && textElements[i] != null)
            {
                labelToElement[labels[i]] = textElements[i];
            }
        }
    }
    public void SetText(int index, string newText)
    {
        if (index >= 0 && index < textElements.Count && textElements[index] != null)
        {
            textElements[index].text = newText;
        }
        else
        {
            Debug.LogWarning($"UIManager: Invalid index or null TextMeshProUGUI at index {index}.");
        }
    } 
    public void SetText(string label, string newText)
    {
        if (labelToElement.TryGetValue(label, out var element) && element != null)
        {
            element.text = newText;
        }
        else
        {
            Debug.LogWarning($"UIManager: Invalid label or null TextMeshProUGUI for label '{label}'.");
        }
    }
  
    public List<TextMeshProUGUI> GetTextElements()
    {
        return textElements;
    }
}
