using System.Threading.Tasks;
using UnityEngine;

public class Projectile : BaseProjectile
{
    private Transform _target;
    private Vector3 _direction;
    private bool _destroyScheduled;

    private void OnEnable()
    {
        _target = null;
        _direction = Vector3.zero;
        _destroyScheduled = false;

        _ = DestroyAfterLifetimeAsync();
    }

    private async Task DestroyAfterLifetimeAsync()
    {
        _destroyScheduled = true;
        float elapsed = 0f;

        while (elapsed < lifetime && _destroyScheduled)
        {
            await Task.Yield(); 
            elapsed += Time.deltaTime;
        }

        if (_destroyScheduled)
            CleanupAndReturn();
    }

    public void Shoot(Transform target)
    {
        base.Shoot();
        _target = target;

        if (_target != null)
            _direction = (_target.position - transform.position).normalized;
    }

    private void Update()
    {
        if (_direction == Vector3.zero) return;

        transform.rotation = Quaternion.LookRotation(_direction);
        transform.position += _direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        _destroyScheduled = false;

        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
            monster.TakeDamage(damage);

        CleanupAndReturn();
    }

    private void CleanupAndReturn()
    {
        _target = null;
        _direction = Vector3.zero;

        if (gameObject.activeSelf)
            PoolManager.Instance.ReturnObject(gameObject);
    }
    private void OnDisable()
    {
        _destroyScheduled = false;
    }
}
