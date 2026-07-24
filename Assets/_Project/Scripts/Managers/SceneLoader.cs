using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressBar;        // A barra de progresso
    [SerializeField] private Text loadingTitleText;     // O texto "Carregando"

    private Coroutine dotsCoroutine;

    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync("PanoramaView"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);

        if (loadingTitleText != null)
            dotsCoroutine = StartCoroutine(AnimateDots());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateProgress(progress);
            yield return null;
        }

        UpdateProgress(1f);
        yield return new WaitForSeconds(0.3f);

        if (dotsCoroutine != null)
            StopCoroutine(dotsCoroutine);

        operation.allowSceneActivation = true;
    }

    void UpdateProgress(float value)
    {
        if (progressBar != null)
            progressBar.value = value;
    }

    IEnumerator AnimateDots()
    {
        int dots = 0;
        while (true)
        {
            dots = (dots + 1) % 4;
            string dotString = new string('.', dots);
            loadingTitleText.text = "Carregando" + dotString;
            yield return new WaitForSeconds(0.4f);
        }
    }
}