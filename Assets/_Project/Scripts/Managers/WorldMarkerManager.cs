using UnityEngine;
using System.Collections.Generic;

public class WorldMarkerManager : MonoBehaviour
{
    [Header("Referências das Setas 3D")]
    [SerializeField] private GameObject forwardMarker;  
    [SerializeField] private GameObject backwardMarker; 

    [Header("Todos os nós da rota (ordem correta)")]
    [SerializeField] private List<PanoramaDataSO> allNodes; 

    void Start()
    {
        if (PanoramaManager.Instance != null)
        {
            PanoramaManager.Instance.OnLocationChanged += UpdateMarkers;
            UpdateMarkers(PanoramaManager.Instance.GetCurrentNode());
        }
    }

    void UpdateMarkers(PanoramaDataSO currentNode)
    {
        if (currentNode == null || allNodes.Count == 0) return;

        
        int currentIndex = allNodes.IndexOf(currentNode);

        if (currentIndex == -1)
        {
            forwardMarker.SetActive(false);
            backwardMarker.SetActive(false);
            return;
        }

        

        bool hasNext = (currentIndex < allNodes.Count - 1);
        bool hasPrev = (currentIndex > 0);

        forwardMarker.SetActive(hasNext);
        backwardMarker.SetActive(hasPrev);
    }
}