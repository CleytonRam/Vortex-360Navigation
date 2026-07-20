using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapControllerGTA : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform mapContainer;   // Container que vai girar
    [SerializeField] private RectTransform playerDot;      // Bolinha vermelha (centro)

    [Header("Dados")]
    [SerializeField] private List<PanoramaDataSO> allNodes; // Todos os nós na ordem

    private Dictionary<PanoramaDataSO, RectTransform> nodeDots = new Dictionary<PanoramaDataSO, RectTransform>();
    private Camera playerCamera;
    private float mapScale;
    private Vector2 centerOffset; // Para centralizar os pontos

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null) playerCamera = FindObjectOfType<Camera>();

        // Calcula a escala e o offset para centralizar
        CalculateMapScaleAndOffset();

        // Desenha os pontos
        DrawAllNodes();

        // Escuta o manager para atualizar posição dos dots
        if (PanoramaManager.Instance != null)
            PanoramaManager.Instance.OnLocationChanged += UpdateNodePositions;

        // Posiciona os dots no estado inicial
        UpdateNodePositions(PanoramaManager.Instance?.GetCurrentNode());

        // Garante que o playerDot fique no centro e visível
        playerDot.anchoredPosition = Vector2.zero;
        playerDot.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        // Atualiza a rotação do minimap continuamente (baseado na câmera)
        if (playerCamera != null)
        {
            float angle = playerCamera.transform.eulerAngles.y;
            mapContainer.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    void CalculateMapScaleAndOffset()
    {
        if (allNodes.Count == 0) { mapScale = 100f; centerOffset = Vector2.zero; return; }

        // Calcula a bounding box dos pontos no espaço do minimap (0 a 1)
        Vector2 min = Vector2.one;
        Vector2 max = Vector2.zero;
        foreach (var node in allNodes)
        {
            if (node.minimapPosition.x < min.x) min.x = node.minimapPosition.x;
            if (node.minimapPosition.y < min.y) min.y = node.minimapPosition.y;
            if (node.minimapPosition.x > max.x) max.x = node.minimapPosition.x;
            if (node.minimapPosition.y > max.y) max.y = node.minimapPosition.y;
        }

        // Centro do bounding box
        Vector2 center = (min + max) * 0.5f;
        // Distância máxima entre dois pontos
        float maxDist = Vector2.Distance(min, max);
        if (maxDist < 0.001f) maxDist = 0.001f;

        // Escala para que a maior distância ocupe 80% do tamanho do container
        float containerSize = Mathf.Min(mapContainer.rect.width, mapContainer.rect.height);
        mapScale = (containerSize * 1.5f) / maxDist;

        // Offset para centralizar o bounding box no centro do container
        centerOffset = center * mapScale;
    }

    void DrawAllNodes()
    {
        // Limpa dots antigos (exceto o playerDot)
        foreach (Transform child in mapContainer)
        {
            if (child.gameObject != playerDot.gameObject)
                Destroy(child.gameObject);
        }

        // Cria um dot para cada nó
        foreach (var node in allNodes)
        {
            GameObject dot = new GameObject($"Dot_{node.name}");
            dot.transform.SetParent(mapContainer, false);

            Image img = dot.AddComponent<Image>();
            img.color = Color.white;

            RectTransform rect = dot.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(8, 8);

            // Guarda a referência para atualizar posição depois
            nodeDots[node] = rect;
        }
    }

    void UpdateNodePositions(PanoramaDataSO currentNode)
    {
        if (currentNode == null) return;

        Vector2 currentPos = currentNode.minimapPosition;

        foreach (var kvp in nodeDots)
        {
            PanoramaDataSO node = kvp.Key;
            RectTransform rect = kvp.Value;

            // Diferença em coordenadas do minimap
            Vector2 diff = node.minimapPosition - currentPos;

            // Posição local no container (considerando escala e offset)
            Vector2 localPos = diff * mapScale; // já está centralizado
            rect.anchoredPosition = localPos;
        }
    }
}