using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIMessageManager : MonoBehaviour
{
    public static UIMessageManager Instance;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float animDuration = 0.5f;

    private Tween showTween;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.zero;
        }
    }

    public void ShowMessage(string message, bool isSpecial = false)
    {
        if (messageText == null) return;

        showTween?.Kill();
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        messageText.transform.localScale = Vector3.zero;

        // Configurações especiais
        if (isSpecial)
        {
            messageText.color = Color.yellow;           
            messageText.fontSize = 58;                  
            messageText.transform.DOScale(1.2f, 0.6f).SetEase(Ease.OutElastic); 
        }
        else
        {
            messageText.color = Color.black;
            messageText.fontSize = 55;
            messageText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        float displayTime = isSpecial ? 5f : displayDuration;
        DOVirtual.DelayedCall(displayTime, () =>
        {
            messageText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
            DOVirtual.DelayedCall(0.3f, () => {
                messageText.gameObject.SetActive(false);
                messageText.color = Color.white; 
                messageText.fontSize = 36;       
            });
        });
    }
}