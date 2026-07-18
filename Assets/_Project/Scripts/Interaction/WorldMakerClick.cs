using UnityEngine;

public class WorldMarkerClick : MonoBehaviour
{
    [Header("Direção que este marcador representa")]
    public Vector2 direction; // Ex: Vector2.up para Frente, Vector2.down para Trás

    // OnMouseDown é chamado automaticamente quando o usuário clica no objeto com o mouse
    void OnMouseDown()
    {
        if (PanoramaManager.Instance != null)
        {
            Debug.Log($"Clicou no marcador! Navegando para direção: {direction}");
            PanoramaManager.Instance.MoveToNeighbor(direction);
        }
    }
}