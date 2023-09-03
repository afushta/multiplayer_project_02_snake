using UnityEngine;


public class DeathFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticlesPrefab;

    private SnakeSkin _skin;

    private void Start()
    {
        _skin = GetComponent<SnakeSkin>();
    }

    private void OnDestroy()
    {
        ParticleSystem particleSystem = Instantiate(_deathParticlesPrefab, transform.position, transform.rotation);
        if (_skin)
        {
            particleSystem.GetComponent<ParticleSystemRenderer>().sharedMaterial = _skin.Material;
        }
    }
}
