using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifetime = 3f;

    [Header("VFX")]
    [SerializeField] private GameObject hitVFX;

    private float _timer;
    private AudioManager _audioManager;

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    private void OnEnable()
    {
        _timer = lifetime;
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SpawnVFX();
            _audioManager?.PlayEnemyHit();
        }

        if (other.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(1);

        Destroy(gameObject);
    }

    private void SpawnVFX()
    {
        if (hitVFX == null)
            return;

        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(vfx, 2f);
    }
}