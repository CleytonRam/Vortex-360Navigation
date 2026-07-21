using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField] private float rotationSpeed = 2f;
    private Vector2 _rotation = Vector2.zero;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minFOV = 30f;
    [SerializeField] private float maxFOV = 90f;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.GetAxis("Mouse X") * rotationSpeed;
            float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed;

            _rotation.x += deltaX;
            _rotation.y -= deltaY;
            _rotation.y = Mathf.Clamp(_rotation.y, -85f, 85f);

            transform.localRotation = Quaternion.Euler(_rotation.y, _rotation.x, 0f);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _cam.fieldOfView -= scroll * zoomSpeed;
            _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView, minFOV, maxFOV);
        }
    }
}