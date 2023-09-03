using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeTail _tailPrefab;
    [SerializeField] private Transform _head;
    [SerializeField] private SnakeMouth _mouth;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Material _baseSnakeMaterial;
    [SerializeField] private int _playerLayer;

    private SnakeTail _tail;
    private string _playerId;

    public void Init(int segmentsCount, string colorString, string playerId, bool isPlayer)
    {
        _playerId = playerId;

        SnakeSkin.PrepareMaterial(colorString, _baseSnakeMaterial, out Material material);
        GetComponent<SnakeSkin>().UpdateMaterial(material);

        _tail = Instantiate(_tailPrefab, transform.position - transform.forward, transform.rotation);
        _tail.Init(_head, segmentsCount, material);

        if (isPlayer)
        {
            _mouth.enabled = true;
            gameObject.layer = _playerLayer;
            _head.gameObject.layer = _playerLayer;
        }
    }

    public void SetSegmentsCount(int segmentsCount)
    {
        _tail.SetSegmentsCount(segmentsCount);
    }

    public void Destroy()
    {
        DestroyDataPosition[] positions = _tail.GetSegmentsPositions();
        DestroyData data = new DestroyData()
        {
            playerId = _playerId,
            positions = positions
        };
        string dataJson = JsonUtility.ToJson(data);
        MultiplayerManager.Instance.SendMessage("destroy", dataJson);

        _tail.Destroy();
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += _speed * Time.deltaTime * _head.forward;
    }
    public void LookAt(Vector3 point)
    {
        _head.LookAt(point);
    }
}

[Serializable]
public class DestroyData
{
    public string playerId;
    public DestroyDataPosition[] positions;
}


[Serializable]
public class DestroyDataPosition
{
    public float x;
    public float z;
}