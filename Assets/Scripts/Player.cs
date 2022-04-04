using UnityEngine;
using System;

public class Player : MonoBehaviour, IDamagable, ICubePicker
{
    [SerializeField]
    private int _maximumHealth;
    public int MaximumHealth { get; private set; }
    public int Health { get; private set; }
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _pickUpClip;
    public event Action<int, int> DamageTaken;
    public event Action Death;
    public event Action<CubeColor> CubePicked;

    private void Awake()
    {
        MaximumHealth = _maximumHealth;
        Health = MaximumHealth;
    }
    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        if (Health <= 0)
        {
            OnDeath();
        }
        DamageTaken?.Invoke(damageValue, Health);
    }
    public void PickUpCube(CubeColor cubeColor) 
    {
        _audioSource.PlayOneShot(_pickUpClip);
        CubePicked?.Invoke(cubeColor);
    }
    private void OnDeath()
    {
        Death?.Invoke();
    }
}
