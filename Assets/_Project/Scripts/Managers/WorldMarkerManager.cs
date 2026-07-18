using UnityEngine;
using System.Collections.Generic;

public class WorldMarkerManager : MonoBehaviour
{
    [Header("Referências das Setas 3D")]
    [SerializeField] private GameObject forwardMarker;  // Seta da frente (verde/azul)
    [SerializeField] private GameObject backwardMarker; // Seta de trás (vermelha)

    [Header("Todos os nós da rota (ordem correta)")]
    [SerializeField] private List<PanoramaDataSO> allNodes; // Arraste Node_01 a Node_15 aqui

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

        // Descobre a posição (índice) do nó atual na lista global
        int currentIndex = allNodes.IndexOf(currentNode);

        // Se não encontrou, esconde tudo
        if (currentIndex == -1)
        {
            forwardMarker.SetActive(false);
            backwardMarker.SetActive(false);
            return;
        }

        // Lógica simples:
        // - Se for o PRIMEIRO nó (índice 0) -> mostra só a seta da frente (se houver próximo)
        // - Se for o ÚLTIMO nó (índice allNodes.Count - 1) -> mostra só a seta de trás (se houver anterior)
        // - Se for do MEIO -> mostra as duas setas

        bool hasNext = (currentIndex < allNodes.Count - 1);
        bool hasPrev = (currentIndex > 0);

        forwardMarker.SetActive(hasNext);
        backwardMarker.SetActive(hasPrev);
    }
}