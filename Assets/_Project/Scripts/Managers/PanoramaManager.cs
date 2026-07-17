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

        // W ou Seta Cima = tenta ir para o primeiro vizinho (Norte)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToNeighbor(Vector2.up);
        }
        // S ou Seta Baixo = tenta ir para o segundo vizinho (Sul) - só pra testar
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToNeighbor(Vector2.down);
        }
        // A ou Esquerda = terceiro vizinho
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToNeighbor(Vector2.left);
        }
        // D ou Direita = quarto vizinho
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToNeighbor(Vector2.right);
        }
    }

    // Função que tenta andar para um vizinho baseado na direção
    public void MoveToNeighbor(Vector2 direction)
    {
        if (_currentNode == null || _currentNode.neighbors == null) return;

        // --- LÓGICA TEMPORÁRIA (mas já é um GRAFO!) ---
        // Como ainda não temos coordenadas geográficas (GPS), 
        // a tecla W vai pegar o VIZINHO 0, a tecla S o VIZINHO 1, 
        // a tecla A o VIZINHO 2, e a tecla D o VIZINHO 3.
        // Depois que baixarmos as 15 imagens, a gente melhora isso com matemática.

        int index = 0;
        if (direction == Vector2.up) index = 0;
        else if (direction == Vector2.down) index = 1;
        else if (direction == Vector2.left) index = 2;
        else if (direction == Vector2.right) index = 3;

        if (index < _currentNode.neighbors.Count)
        {
            ChangeLocation(_currentNode.neighbors[index]);
        }
        else
        {
            Debug.Log($"Não tem vizinho na direção {direction} (índice {index})");
        }
    }

    // Função pública para pegar o nó atual (usado pelo Raycaster)
    public PanoramaDataSO GetCurrentNode() => _currentNode;
}