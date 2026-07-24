using UnityEngine;
using DG.Tweening;

public class GnomoCollectible : MonoBehaviour
{
    public System.Action OnCollected;
    private bool isCollecting = false; 
    void OnMouseDown()
    {
        if (Time.timeScale == 0 || isCollecting) return;

        isCollecting = true;

        AudioManager.Instance?.PlayCollect();

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                OnCollected?.Invoke();
            });
    }
}