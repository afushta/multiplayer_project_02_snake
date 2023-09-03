using UnityEngine;


public class SnakeMouth : MonoBehaviour
{
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _angleEpsilon = 5f;
    [SerializeField] private LayerMask _layerMask;

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Apple apple))
            {
                apple.Collect();
            }
            else if (collider.GetComponentInParent<Snake>())
            {
                Transform enemy = collider.transform;
                float playerAngle = Vector3.Angle(transform.position - enemy.position, transform.forward);
                float enemyAngle = Vector3.Angle(enemy.position - transform.position, enemy.forward);
                if (playerAngle < enemyAngle + _angleEpsilon)
                {
                    Debug.Log(collider.gameObject);
                    GameManager.Instance.GameOver();
                }
            }
            else
            {
                Debug.Log(collider.gameObject);
                GameManager.Instance.GameOver();
            }
        }
    }
}
