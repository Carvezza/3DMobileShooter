using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : GameObjectFactory
{
    [SerializeField]
    Cube _redCubePrefab;
    [SerializeField]
    Cube _greenCubePrefab;
    [SerializeField]
    Cube _yellowCubePrefab;
    [SerializeField]
    protected List<Cube> _redPool;
    [SerializeField]
    protected List<Cube> _greenPool;
    [SerializeField]
    protected List<Cube> _yellowPool;

    public virtual Cube Instantiate(CubeColor color)
    {
        Cube cube = GetFromPool(color);
        if (cube == null)
        {
            cube = Expand(color);
        }
        return cube;
    }
    private Cube GetFromPool(CubeColor color)
    {
        List<Cube> pool = color switch
        {
            CubeColor.Red => _redPool,
            CubeColor.Green => _greenPool,
            CubeColor.Yellow => _yellowPool,
            _ => _redPool
        };
        foreach (Cube item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }
    protected virtual Cube Expand(CubeColor color)
    {
        Cube cube = color switch
        {
            CubeColor.Red => CreateGameObjectInstance<Cube>(_redCubePrefab),
            CubeColor.Green => CreateGameObjectInstance<Cube>(_greenCubePrefab),
            CubeColor.Yellow => CreateGameObjectInstance<Cube>(_yellowCubePrefab),
            _ => CreateGameObjectInstance<Cube>(_redCubePrefab)
        };
        List<Cube> pool = color switch
        {
            CubeColor.Red => _redPool,
            CubeColor.Green => _greenPool,
            CubeColor.Yellow => _yellowPool,
            _ => _redPool
        };
        cube.OriginFactory = this;
        pool.Add(cube);
        return cube;
    }
    public void Reclaim(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }
}
