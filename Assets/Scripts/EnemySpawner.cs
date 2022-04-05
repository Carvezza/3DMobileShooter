using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory _enemyFactory;
    [SerializeField]
    private CubeFactory _cubeFactory;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _spawnCoolDown;
    [SerializeField]
    private Vector2 _origin;
    [SerializeField]
    private Vector2 _size;
    [SerializeField]
    private float _minimumDistanceToPlayer;
    [SerializeField]
    private Camera _cameraToAvoid;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    private Vector3 RandomVector3()
    {
        float x = Random.Range(_origin.x - _size.x / 2, _origin.x + _size.x / 2);
        float z = Random.Range(_origin.y - _size.y / 2, _origin.y + _size.y / 2);
        return new Vector3(x, 0, z);
    }
    private void SpawnAndPlace(Vector3 position)
    {
        Enemy enemy = _enemyFactory.Instantiate();
        enemy.Init(position, Quaternion.LookRotation(_player.position - transform.position, Vector3.up), _player, _cubeFactory);
    }
    private bool Validate(Vector3 value)
    {
        Vector3 valueOnScreen = _cameraToAvoid.WorldToViewportPoint(value);
        // Out of view
        if (valueOnScreen.z > 0 && valueOnScreen.x > 0 && valueOnScreen.x < 1 && valueOnScreen.y > 0 && valueOnScreen.y < 1)
        {
            return false;
        }
        // Far enough
        if (Vector3.Distance(_player.position, value) < _minimumDistanceToPlayer)
        {
            return false;
        }
        // On navmesh
        if (!NavMesh.SamplePosition(value, out _, 1f, NavMesh.AllAreas))
        {
            return false;
        }
        return true;
    }
    private bool TrySpawn()
    {
        Vector3 point = RandomVector3();
        if (Validate(point))
        {
            SpawnAndPlace(point);
            return true;
        }
        return false;
    }
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            while (!TrySpawn());
            yield return new WaitForSeconds(_spawnCoolDown);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_player.position, _minimumDistanceToPlayer);
    }
}
