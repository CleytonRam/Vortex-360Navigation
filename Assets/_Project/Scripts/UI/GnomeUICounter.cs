using UnityEngine;
using TMPro; 

public class GnomeUICounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText; 
    [SerializeField] private int maxGnomes = 10;

    void Start()
    {
        UpdateCounter(0);
    }

    public void UpdateCounter(int current)
    {
        if (counterText != null)
            counterText.text = $"Gnomos: {current}/{maxGnomes}";
    }

    public void SetCompletedColor()
    {
        if (counterText != null)
            counterText.color = Color.yellow;
    }
}