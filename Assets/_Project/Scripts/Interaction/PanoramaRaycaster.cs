using UnityEngine;

public class PanoramaRaycaster : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private LayerMask sphereLayer; // Deixe como "Everything" ou "Default"

    private Camera _cam;

    void Start()
    {
        // Pega a câmera local (mais seguro)
        _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;

        if (_cam == null)
            Debug.LogError("PanoramaRaycaster: Nenhuma câmera encontrada!");
    }

    void Update()
    {
        if (_cam == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Cria o raio da câmera até o mouse
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            // --- DEBUG VISUAL: Desenha uma linha vermelha no mundo (3D) ---
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);

            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            if (hits.Length > 0)
            {
                foreach (var h in hits)
                {
                    Debug.Log($"Bateu em: {h.collider.gameObject.name} (Layer: {h.collider.gameObject.layer})");
                }

                // Se bateu em algo, navega
                if (PanoramaManager.Instance != null)
                    PanoramaManager.Instance.MoveToNeighbor(Vector2.up);
            }
            else
            {
                Debug.Log("Raycast NÃO acertou NADA.");
            }

        }
    }
}