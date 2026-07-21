using UnityEngine;
using System.Collections.Generic;

public class PanoramaManager : MonoBehaviour
{
    public static PanoramaManager Instance { get; private set; }

    [Header("Configurações")]
    [SerializeField] private Material panoramaMaterial; 
    [SerializeField] private PanoramaDataSO startingNode; 

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

    public void ChangeLocation(PanoramaDataSO targetNode)
    {
        if (targetNode == null) return;
        if (targetNode == _currentNode) return; 

        panoramaMaterial.mainTexture = targetNode.panoramaTexture;

        _currentNode = targetNode;

        OnLocationChanged?.Invoke(_currentNode);

        Debug.Log($"Chegou em: {targetNode.name}");
    }

    void Update()
    {
        if (Time.timeScale == 0f || _currentNode == null) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToNeighbor(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToNeighbor(Vector2.down);
        }
    }

    public void MoveToNeighbor(Vector2 direction)
    {
        if (_currentNode == null || _currentNode.neighbors == null) return;

        int count = _currentNode.neighbors.Count;
        if (count == 0) return;

        int index = -1;

        if (direction == Vector2.up)
        {
            index = 0; 
        }
        else if (direction == Vector2.down)
        {
           
            if (count > 1) index = 1;
            else if (count == 1) index = 0;
        }

        if (index >= 0 && index < count)
        {
            ChangeLocation(_currentNode.neighbors[index]);
        }
        else
        {
            Debug.Log($"Não tem vizinho disponível para a direção {direction}");
        }
    }

    public PanoramaDataSO GetCurrentNode() => _currentNode;
}