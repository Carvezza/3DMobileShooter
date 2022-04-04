using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _coolDown;
    private float _nextHitTime;
    [SerializeField]
    LayerMask _toHitMask;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _attackClip;

    private void OnTriggerEnter(Collider other)
    {
        if ((_toHitMask | (1 << other.gameObject.layer)) == _toHitMask)
        {
            DealDamage(other.GetComponent<IDamagable>());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((_toHitMask | (1 << other.gameObject.layer)) == _toHitMask)
        {
            DealDamage(other.GetComponent<IDamagable>());
        }
    }
    private void DealDamage(IDamagable target)
    {
        if (Time.time > _nextHitTime)
        {
            _audioSource.PlayOneShot(_attackClip);
            target.TakeDamage(_damage);
            _nextHitTime = Time.time + _coolDown;
        }
    }
}
