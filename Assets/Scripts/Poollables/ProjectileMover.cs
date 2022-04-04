using System.Collections;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f,100f)]
    private float speed;
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private LayerMask _layersToHit;
    [SerializeField]
    private LayerMask _layersToDeSpawn;
    [SerializeField]
    private int _damage;
    public ProjectileFactory OriginFactory { get; set; }

    public void Init(Vector3 position, Quaternion orientation)
    {
        transform.SetPositionAndRotation(position, orientation);
        StartCoroutine(TimedSelfDestruct());
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * speed * Time.fixedDeltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((_layersToHit | (1 << other.gameObject.layer)) == _layersToHit)
        {
            other.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
        }
        if ((_layersToDeSpawn | (1 << other.gameObject.layer)) == _layersToDeSpawn)
        {
            DeSpawn();
        }
    }
    private void DeSpawn()
    {
        StopAllCoroutines();
        OriginFactory.Reclaim(this);
    }
    private IEnumerator TimedSelfDestruct()
    {
        yield return new WaitForSeconds(_lifeTime);
        DeSpawn();
    }
}
