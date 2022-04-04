using UnityEngine;

public abstract class GameObjectFactory : MonoBehaviour
{
    protected virtual T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
    {
        T instance = Instantiate(prefab);
        return instance;
    }
    protected virtual void Reclaim<T>(T product) where T : MonoBehaviour
    {
        GameObject.Destroy(product.gameObject);
    }
}
