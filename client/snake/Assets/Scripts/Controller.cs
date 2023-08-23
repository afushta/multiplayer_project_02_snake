using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Transform _cursorOrigin;
    [SerializeField] private Transform _cursor;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _distanceFromSnake = 1f;

    private Transform _snake;
    private Camera _camera;
    private Plane _plane;
    private Vector3 _targetDirection = Vector3.forward;

    public void Init(Transform snake)
    {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _snake = snake;
        _cursor.localPosition = Vector3.forward * _distanceFromSnake;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateTargetDirection();
        }

        MoveCursor();
        SendMovement();
    }

    private void UpdateTargetDirection()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);
        Vector3 hitPoint = ray.GetPoint(distance);
        _targetDirection = (hitPoint - _cursorOrigin.position).normalized;
    }

    private void MoveCursor()
    {
        _cursorOrigin.position = _snake.position;
        _cursorOrigin.rotation = Quaternion.RotateTowards(_cursor.rotation, Quaternion.LookRotation(_targetDirection), _rotationSpeed * Time.deltaTime);
    }

    private void SendMovement()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "x", _cursor.position.x },
            { "z", _cursor.position.z },
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
