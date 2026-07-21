using UnityEngine;

public class GnomoCollectible : MonoBehaviour
{
    public System.Action OnCollected;

    void OnMouseDown()
    {
        if (Time.timeScale == 0) return; 

        AudioManager.Instance?.PlayCollect();
        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}