using UnityEngine;
using System.Collections.Generic;

public class PanoramaManager : MonoBehaviour
{
    public static PanoramaManager Instance { get; private set; }

    [Header("Configurações")]
    [SerializeField] private Material panoramaMaterial; // O material da esfera
    [SerializeField] private PanoramaDataSO startingNode; // O ponto de partida

    // Evento para avisar o Minimapa que mudou
    public System.Action<PanoramaDataSO> OnLocationChanged;

    private PanoramaDataSO _currentNode;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (startingNode != null)
            ChangeLocation(startingNode);
        else
            Debug.LogWarning("Nenhum ponto inicial configurado!");
    }

    // Função principal para trocar de foto
    public void ChangeLocation(PanoramaDataSO targetNode)
    {
        if (targetNode == null) return;
        if (targetNode == _currentNode) return; // Evita trocar pra mesma foto

        // Troca a textura no material da esfera
        panoramaMaterial.mainTexture = targetNode.panoramaTexture;

        // Atualiza o nó atual
        _currentNode = targetNode;

        // Dispara o evento (quem escutar vai reagir)
        OnLocationChanged?.Invoke(_currentNode);

        Debug.Log($"Chegou em: {targetNode.name}");
    }

    // Navegação por TECLADO (Street View)
    void Update()
    {
        if (_currentNode == null) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToNeighbor(Vector2.up);   // Avançar (índice 0)
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToNeighbor(Vector2.down); // Voltar (índice 1)
        }
    }

    // Função que tenta andar para um vizinho baseado na direção
    public void MoveToNeighbor(Vector2 direction)
    {
        if (_currentNode == null || _currentNode.neighbors == null) return;

        int count = _currentNode.neighbors.Count;
        if (count == 0) return;

        int index = -1;

        // Lógica para "Frente" (W ou Seta Cima)
        if (direction == Vector2.up)
        {
            index = 0; // Sempre tenta o primeiro vizinho
        }
        // Lógica para "Trás" (S ou Seta Baixo)
        else if (direction == Vector2.down)
        {
            // Se tiver 2 ou mais vizinhos, usa o índice 1 (trás)
            if (count > 1) index = 1;
            // Se tiver SÓ 1 vizinho (fim da rua), usa o índice 0 (único caminho disponível)
            else if (count == 1) index = 0;
        }

        // Executa a navegação
        if (index >= 0 && index < count)
        {
            ChangeLocation(_currentNode.neighbors[index]);
        }
        else
        {
            Debug.Log($"Não tem vizinho disponível para a direção {direction}");
        }
    }

    // Função pública para pegar o nó atual (usado pelo Raycaster)
    public PanoramaDataSO GetCurrentNode() => _currentNode;
}