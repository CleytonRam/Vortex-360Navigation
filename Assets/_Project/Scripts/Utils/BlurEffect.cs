using UnityEngine;

public class BlurEffect : MonoBehaviour
{
    [SerializeField] private Material blurMaterial;
    [SerializeField] private float maxBlur = 3f;    // Intensidade máxima do blur
    [SerializeField] private float duration = 0.3f;  // Duração do efeito

    private float currentBlur = 0f;
    private bool isBlurring = false;
    private float timer = 0f;

    void Update()
    {
        if (isBlurring)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            // Aplica o blur com easing (entrada e saída suave)
            float blurAmount = Mathf.Sin(progress * Mathf.PI) * maxBlur;
            blurMaterial.SetFloat("_BlurSize", blurAmount);

            if (progress >= 1f)
            {
                isBlurring = false;
                blurMaterial.SetFloat("_BlurSize", 0f);
            }
        }
    }

    public void TriggerBlur()
    {
        timer = 0f;
        isBlurring = true;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (blurMaterial != null)
            Graphics.Blit(src, dest, blurMaterial);
        else
            Graphics.Blit(src, dest);
    }
}