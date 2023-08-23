using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        float offsetY = _cameraTransform.position.y;
        _cameraTransform.parent = transform;
        _cameraTransform.localPosition = offsetY * Vector3.up;
    }

    private void OnDestroy()
    {
        if (Camera.main) _cameraTransform.parent = null;
    }
}
