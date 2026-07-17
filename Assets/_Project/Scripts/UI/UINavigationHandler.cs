using UnityEngine;

public class UINavigationHandler : MonoBehaviour
{
    public void GoUp() => PanoramaManager.Instance?.MoveToNeighbor(Vector2.up);
    public void GoDown() => PanoramaManager.Instance?.MoveToNeighbor(Vector2.down);
    public void GoLeft() => PanoramaManager.Instance?.MoveToNeighbor(Vector2.left);
    public void GoRight() => PanoramaManager.Instance?.MoveToNeighbor(Vector2.right);
}