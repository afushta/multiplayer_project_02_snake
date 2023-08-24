using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private Transform _head;
    [SerializeField] private float _speed = 2f;

    private Tail _tail;

    public void Init(int segmentsCount, Material material)
    {
        _tail = Instantiate(_tailPrefab, transform.position, transform.rotation);
        _tail.Init(_head, segmentsCount, material);

        GetComponent<Skin>().UpdateMaterial(material);
    }

    public void SetSegmentsCount(int segmentsCount)
    {
        _tail.SetSegmentsCount(segmentsCount);
    }

    public void Destroy()
    {
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
