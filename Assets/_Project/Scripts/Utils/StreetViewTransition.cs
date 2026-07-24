using UnityEngine;
using DG.Tweening;

public class StreetViewTransition : MonoBehaviour
{
    public static StreetViewTransition Instance;

    [Header("Câmera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float stepDuration = 0.4f;
    [SerializeField] private float forwardDistance = 0.3f; // Distância do passo
    [SerializeField] private float fovPulse = 5f;

    private Vector3 originalPos;
    private float originalFOV;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (playerCamera == null) playerCamera = Camera.main;
        originalPos = playerCamera.transform.localPosition;
        originalFOV = playerCamera.fieldOfView;
    }

    public void DoStep(System.Action onComplete)
    {
        BlurEffect blur = FindObjectOfType<BlurEffect>();
        if (blur != null) blur.TriggerBlur();
        
        Vector3 lookDirection = playerCamera.transform.forward;

        // Mantém a altura fixa (Y = 0) para não flutuar
        lookDirection.y = 0;
        lookDirection.Normalize();

        // Calcula a posição alvo (avança na direção olhada)
        Vector3 targetPos = originalPos + lookDirection * forwardDistance;

        // ---- ANIMAÇÃO ----
        Sequence seq = DOTween.Sequence();

        // Move e aumenta o FOV
        seq.Append(playerCamera.transform.DOLocalMove(targetPos, stepDuration * 0.6f).SetEase(Ease.OutQuad));
        seq.Join(playerCamera.DOFieldOfView(originalFOV + fovPulse, stepDuration * 0.6f).SetEase(Ease.OutQuad));

        // Troca a textura no meio do movimento
        seq.InsertCallback(stepDuration * 0.4f, () =>
        {
            onComplete?.Invoke();
        });

        // Volta à posição original
        seq.Append(playerCamera.transform.DOLocalMove(originalPos, stepDuration * 0.4f).SetEase(Ease.InQuad));
        seq.Join(playerCamera.DOFieldOfView(originalFOV, stepDuration * 0.4f).SetEase(Ease.InQuad));

        seq.Play();
    }
}