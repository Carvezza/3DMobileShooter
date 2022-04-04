using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    Enemy _enemyPrefab;
    [SerializeField]
    protected List<Enemy> _pool;

    public virtual Enemy Instantiate()
    {
        Enemy enemy = GetFromPool();
        if (enemy == null)
        {
            enemy = Expand();
        }
        return enemy;
    }
    private Enemy GetFromPool()
    {
        foreach (Enemy item in _pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }
    protected virtual Enemy Expand()
    {
        Enemy enemy = CreateGameObjectInstance<Enemy>(_enemyPrefab);
        enemy.OriginFactory = this;
        _pool.Add(enemy);
        return enemy;
    }
    public void Reclaim(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
