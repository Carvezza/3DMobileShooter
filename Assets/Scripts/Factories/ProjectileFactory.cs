using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : GameObjectFactory
{
    [SerializeField]
    ProjectileMover _redProjectilePrefab;
    [SerializeField]
    ProjectileMover _greenProjectilePrefab;
    [SerializeField]
    ProjectileMover _yellowProjectilePrefab;
    [SerializeField]
    protected List<ProjectileMover> _redPool;
    [SerializeField]
    protected List<ProjectileMover> _greenPool;
    [SerializeField]
    protected List<ProjectileMover> _yellowPool;

    public virtual ProjectileMover Instantiate(ProjectileColor color)
    {
        ProjectileMover projectile = GetFromPool(color);
        if (projectile == null)
        {
            projectile = Expand(color);
        }
        return projectile;
    }
    private ProjectileMover GetFromPool(ProjectileColor color)
    {
        List<ProjectileMover> pool = color switch
        {
            ProjectileColor.Red => _redPool,
            ProjectileColor.Green => _greenPool,
            ProjectileColor.Yellow => _yellowPool,
            _ => _redPool
        };
        foreach (ProjectileMover item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }

    protected virtual ProjectileMover Expand(ProjectileColor color)
    {
        ProjectileMover projectile = color switch
        {
            ProjectileColor.Red => CreateGameObjectInstance<ProjectileMover>(_redProjectilePrefab),
            ProjectileColor.Green => CreateGameObjectInstance<ProjectileMover>(_greenProjectilePrefab),
            ProjectileColor.Yellow => CreateGameObjectInstance<ProjectileMover>(_yellowProjectilePrefab),
            _ => CreateGameObjectInstance<ProjectileMover>(_redProjectilePrefab)
        };
        List<ProjectileMover> pool = color switch
        {
            ProjectileColor.Red => _redPool,
            ProjectileColor.Green => _greenPool,
            ProjectileColor.Yellow => _yellowPool,
            _ => _redPool
        };
        projectile.OriginFactory = this;
        pool.Add(projectile);
        return projectile;
    }
    public void Reclaim(ProjectileMover mover)
    {
        mover.gameObject.SetActive(false);
    }
}
