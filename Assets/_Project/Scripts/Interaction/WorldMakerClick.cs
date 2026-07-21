using UnityEngine;

public class WorldMarkerClick : MonoBehaviour
{
    [Header("Direção que este marcador representa")]
    public Vector2 direction; 

   
    void OnMouseDown()
    {
        if (Time.timeScale == 0f) return;
        if (PanoramaManager.Instance != null)
        {
            AudioManager.Instance?.PlayClick();
            Debug.Log($"Clicou no marcador! Navegando para direção: {direction}");
            PanoramaManager.Instance.MoveToNeighbor(direction);
        }
    }
}