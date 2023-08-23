using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] private Transform _segmentPrefab;
    [SerializeField] private float _segmentsDistance = 1f;

    private Transform _head;
    private List<Transform> _segments;
    private List<MovementSnapshot> _movementHistory;

    public void Init(Transform head, int segmentsCount)
    {
        _head = head;
        _segments = new List<Transform> { transform };
        _movementHistory = new List<MovementSnapshot>
        {
            new MovementSnapshot(_head)
        };

        SetSegmentsCount(segmentsCount);

        _movementHistory.Add(new MovementSnapshot(transform));
    }

    public void Destroy()
    {
        foreach (Transform segment in _segments)
        {
            Destroy(segment.gameObject);
        }
    }

    public void SetSegmentsCount(int value)
    {
        int diff = _segments.Count - value - 1;
        if (diff < 0)
        {
            for (int i = 0; i < -diff; i++)
            {
                AddSegment();
            }
        }
        else if(diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                RemoveSegment();
            }
        }
    }

    private void AddSegment()
    {
        Transform segment = Instantiate(_segmentPrefab, transform.position, transform.rotation);
        _segments.Insert(0, segment);
        _movementHistory.Add(new MovementSnapshot(transform));
    }

    private void RemoveSegment()
    {
        if (_segments.Count <= 1) return;

        Transform segment = _segments[0];
        _segments.RemoveAt(0);
        _movementHistory.RemoveAt(_movementHistory.Count - 1);
        Destroy(segment.gameObject);
    }

    private void LateUpdate()
    {
        Vector3 currentPosition = _head.position;
        float distance = (currentPosition - _movementHistory[0].position).magnitude;
        
        while (distance > _segmentsDistance)
        {
            Vector3 direction = (currentPosition - _movementHistory[0].position).normalized;
            MovementSnapshot snapshot = new (_movementHistory[0].position + direction * _segmentsDistance, _head.rotation);
            _movementHistory.Insert(0, snapshot);
            _movementHistory.RemoveAt(_movementHistory.Count - 1);

            distance -= _segmentsDistance;
        }

        for (int i = 0; i < _segments.Count; i++)
        {
            MovementSnapshot from = _movementHistory[i + 1];
            MovementSnapshot to = _movementHistory[i];
            float percent = distance / _segmentsDistance;
            _segments[i].position = Vector3.Lerp(from.position, to.position, percent);
            _segments[i].rotation = Quaternion.Lerp(from.rotation, to.rotation, percent);
        }
    }
}


public struct MovementSnapshot
{
    public Vector3 position;
    public Quaternion rotation;

    public MovementSnapshot(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public MovementSnapshot(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
    }
}
