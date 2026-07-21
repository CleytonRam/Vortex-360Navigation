using UnityEngine;
using TMPro; // <-- IMPORTANTE: adicione isso
using System.Collections;

public class UIMessageManager : MonoBehaviour
{
    public static UIMessageManager Instance;

    [SerializeField] private TextMeshProUGUI messageText; 
    [SerializeField] private float displayDuration = 3f;

    private Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (messageText == null) return;

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        messageText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}