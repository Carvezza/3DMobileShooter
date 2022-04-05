using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    Transform _target;
    [SerializeField]
    EnemyMeleeWeapon _weapon;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth;
    private CubeFactory _cubeFactory;
    public EnemyFactory OriginFactory { get; set; }
    [SerializeField]
    Transform _cubeSpawnPoint;
    [SerializeField]
    [Range(0.1f,1f)]
    private float _rePathInterval;
    [SerializeField]
    private NavMeshAgent _agent;
    public event Action<int, int> DamageTaken;
    public event Action Death;
    [SerializeField]
    private bool _active;

    public void Init(Vector3 position, Quaternion orientation, Transform target, CubeFactory cubeFactory)
    {
        transform.position = position;
        transform.rotation = orientation;
        _target = target;
        _cubeFactory = cubeFactory;
        StartCoroutine(Repath());
    }
    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        if (_health <= 0)
        {
            OnDeath();
        }
        DamageTaken?.Invoke(damageValue, _health);
    }
    private void OnDeath()
    {
        CubeColor color = (CubeColor)Random.Range(0, 3);
        Cube cube = _cubeFactory.Instantiate(color);
        cube.Init(_cubeSpawnPoint.position, _cubeSpawnPoint.rotation);
        DeSpawn();
        Death?.Invoke();
    }
    private void ResetHealth() => _health = _maxHealth;
    private void DeSpawn()
    {
        ResetHealth();
        StopAllCoroutines();
        OriginFactory.Reclaim(this);
    }
    private IEnumerator Repath()
    {
        while (_active)
        {
            _agent.SetDestination(_target.position);
            yield return new WaitForSeconds(_rePathInterval);
        }
    }
}
