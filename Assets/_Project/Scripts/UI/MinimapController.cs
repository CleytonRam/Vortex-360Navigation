using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapControllerGTA : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform mapContainer;   
    [SerializeField] private RectTransform playerDot;      

    [Header("Dados")]
    [SerializeField] private List<PanoramaDataSO> allNodes; 

    private Dictionary<PanoramaDataSO, Image> nodeDots = new Dictionary<PanoramaDataSO, Image>();
    private Camera playerCamera;
    private float mapScale;
    private Vector2 centerOffset; 

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null) playerCamera = FindObjectOfType<Camera>();

        CalculateMapScaleAndOffset();

        DrawAllNodes();

        if (PanoramaManager.Instance != null)
            PanoramaManager.Instance.OnLocationChanged += UpdateNodePositions;

        UpdateNodePositions(PanoramaManager.Instance?.GetCurrentNode());

        playerDot.anchoredPosition = Vector2.zero;
        playerDot.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (playerCamera != null)
        {
            float angle = playerCamera.transform.eulerAngles.y;
            mapContainer.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    void CalculateMapScaleAndOffset()
    {
        if (allNodes.Count == 0) { mapScale = 100f; centerOffset = Vector2.zero; return; }

        Vector2 min = Vector2.one;
        Vector2 max = Vector2.zero;
        foreach (var node in allNodes)
        {
            if (node.minimapPosition.x < min.x) min.x = node.minimapPosition.x;
            if (node.minimapPosition.y < min.y) min.y = node.minimapPosition.y;
            if (node.minimapPosition.x > max.x) max.x = node.minimapPosition.x;
            if (node.minimapPosition.y > max.y) max.y = node.minimapPosition.y;
        }

        Vector2 center = (min + max) * 0.5f;
        float maxDist = Vector2.Distance(min, max);
        if (maxDist < 0.001f) maxDist = 0.001f;

        float containerSize = Mathf.Min(mapContainer.rect.width, mapContainer.rect.height);
        mapScale = (containerSize * 1.5f) / maxDist;

        centerOffset = center * mapScale;
    }

    void DrawAllNodes()
    {


        foreach (Transform child in mapContainer)
        {
            if (child.gameObject != playerDot.gameObject)
                Destroy(child.gameObject);
        }

        foreach (var node in allNodes)
        {
            GameObject dot = new GameObject($"Dot_{node.name}");
            dot.transform.SetParent(mapContainer, false);

            Image img = dot.AddComponent<Image>();
            img.color = Color.white;

            RectTransform rect = dot.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(8, 8);

            nodeDots[node] = img;
        }
    }

    void UpdateNodePositions(PanoramaDataSO currentNode)
    {
        if (currentNode == null) return;

        Vector2 currentPos = currentNode.minimapPosition;

        foreach (var kvp in nodeDots)
        {
            PanoramaDataSO node = kvp.Key;
            RectTransform rect = kvp.Value.rectTransform;

            Vector2 diff = node.minimapPosition - currentPos;

            
            Vector2 localPos = diff * mapScale; 
            rect.anchoredPosition = localPos;
        }
    }

    public void SetNodeColor(PanoramaDataSO node, Color color)
    {
        if (nodeDots.ContainsKey(node))
        {
            nodeDots[node].color = color;
        }
    }
}