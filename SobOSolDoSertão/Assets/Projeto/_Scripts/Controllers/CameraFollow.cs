using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Alvo")]
    [SerializeField] private Transform target;

    [Header("Movimentação Suave")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 10f;

    private Camera cam;
    private Vector3 shakeOffset = Vector3.zero;

    // Singleton (acesso global fácil)
    public static CameraFollow Instance { get; private set; }

    // Shake
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        cam = GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("CameraFollow precisa estar em um GameObject com Camera!");

        cam.orthographicSize = maxZoom;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Movimento suave
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Shake
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0; // sem shake em Z
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        // Aplicar posição final
        Vector3 finalPosition = smoothedPosition + shakeOffset;
        finalPosition.z = transform.position.z;
        transform.position = finalPosition;

        // Zoom com scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
        else
        {
            float controllerZoom = Input.GetAxis("Zoom");
            if (Mathf.Abs(controllerZoom) > 0.2f) // filtro de ruído
            {
                float newSize = cam.orthographicSize - controllerZoom * zoomSpeed * Time.deltaTime;
                cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }
        }
    }

    /// <summary>
    /// Chame isso de qualquer lugar para sacudir a câmera.
    /// </summary>
    /// <param name="duration">Tempo do shake</param>
    /// <param name="magnitude">Intensidade</param>
    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
