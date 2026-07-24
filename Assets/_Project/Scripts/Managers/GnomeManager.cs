using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GnomeManager : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject gnomePrefab;
    [SerializeField] private List<PanoramaDataSO> allNodes;
    [SerializeField] private List<Transform> spawnPoints; 
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int maxGnomes = 10;
    [SerializeField] private UIMessageManager uiMessage;
    [SerializeField] private GnomeUICounter uiCounter;


    private GameObject currentGnomeInstance;
    private PanoramaDataSO currentTargetNode;
    private int collectedCount = 0;
    private MinimapControllerGTA minimap;
    private bool isSpawning = false;

    void Start()
    {

        if (uiMessage == null) uiMessage = FindObjectOfType<UIMessageManager>();
        if (uiCounter == null) uiCounter = FindObjectOfType<GnomeUICounter>();
        minimap = FindObjectOfType<MinimapControllerGTA>();
        if (PanoramaManager.Instance != null)
            PanoramaManager.Instance.OnLocationChanged += OnNodeChanged;

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (collectedCount < maxGnomes)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (currentGnomeInstance != null) continue;

            PanoramaDataSO currentNode = PanoramaManager.Instance?.GetCurrentNode();
            if (currentNode == null) yield break;

            List<PanoramaDataSO> available = new List<PanoramaDataSO>(allNodes);
            available.Remove(currentNode);
            if (available.Count == 0) yield break;

            currentTargetNode = available[Random.Range(0, available.Count)];

            if (spawnPoints.Count == 0) yield break;
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];

            minimap?.SetNodeColor(currentTargetNode, Color.green);

            currentGnomeInstance = Instantiate(gnomePrefab, spawn.position, spawn.rotation);
            currentGnomeInstance.SetActive(false);
            AudioManager.Instance?.PlaySpawn(); 
            uiMessage?.ShowMessage("Um gnomo apareceu! Encontre-o!");

            GnomoCollectible gnomo = currentGnomeInstance.GetComponent<GnomoCollectible>();
            if (gnomo != null)
                gnomo.OnCollected += OnGnomoCollected;

            if (PanoramaManager.Instance.GetCurrentNode() == currentTargetNode)
            {
                currentGnomeInstance.SetActive(true);
            }

            Debug.Log($"Gnomo spawnado no nó {currentTargetNode.name} no ponto {spawn.name}. Aguardando jogador chegar ao nó.");
        }
    }

    void OnNodeChanged(PanoramaDataSO newNode)
    {
        if (currentGnomeInstance == null || currentTargetNode == null) return;

        if (newNode == currentTargetNode)
        {
            currentGnomeInstance.SetActive(true);
            Debug.Log("Jogador chegou no nó do gnomo! Gnomo ativado.");
        }
        else
        {
            if (currentGnomeInstance.activeSelf)
            {
                currentGnomeInstance.SetActive(false);
                Debug.Log("Jogador saiu do nó do gnomo. Gnomo desativado (mas ainda existe).");
            }
        }
    }

    void OnGnomoCollected()
    {
        collectedCount++;
        uiCounter?.UpdateCounter(collectedCount);
        Debug.Log($"Gnomo coletado! ({collectedCount}/{maxGnomes})");

        // --- NOVO: Verifica se completou todos ---
        if (collectedCount >= maxGnomes)
        {
            uiMessage?.ShowMessage("Parabéns! Você encontrou todos os gnomos!", true);
            uiCounter?.SetCompletedColor(); // Muda para dourado
            AudioManager.Instance?.PlayCollect();
            return;
        }

        // Resto do código (limpeza do nó atual e destruição do gnomo)
        if (currentTargetNode != null)
            minimap?.SetNodeColor(currentTargetNode, Color.white);

        if (currentGnomeInstance != null)
        {
            Destroy(currentGnomeInstance);
            currentGnomeInstance = null;
        }
        currentTargetNode = null;
    }
}