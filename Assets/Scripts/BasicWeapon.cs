using System.Collections;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public FP_Input playerInput;
    public float shootRate = 0.15F;
    public float reloadTime = 1.0F;
    public int ammoCount = 15;
    private int ammo;
    private float delay;
    private bool reloading;

    [SerializeField]
    ProjectileFactory projectileFactory;
    [SerializeField]
    Transform _muzzle;
    [SerializeField]
    ProjectileColor _projectileColor;
    [SerializeField]
    private Player _player;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    private AudioClip _fireClip;
    [SerializeField]
    private AudioClip _emptyFireClip;
    [SerializeField]
    private AudioClip _reloadClip;

    void Start()
    {
        _player.CubePicked += (c) => _projectileColor = ProjectileColorFromCubeColor(c);
        ammo = ammoCount;
    }
    void Update()
    {
        if (playerInput.Shoot())
            if (Time.time > delay)
                Shoot();
        if (playerInput.Reload())
            if (!reloading && ammoCount < ammo)
                StartCoroutine("Reload");
    }
    void Shoot()
    {
        if (ammoCount > 0)
        {
            _audioSource.clip = _fireClip;
            _audioSource.PlayOneShot(_fireClip);
            ProjectileMover projectile = projectileFactory.Instantiate(_projectileColor);
            projectile.Init(_muzzle.position, _muzzle.rotation);
            ammoCount--;
        }
        else
        {
            _audioSource.clip = _emptyFireClip;
            _audioSource.Play();
        }
        delay = Time.time + shootRate;
    }
    IEnumerator Reload()
    {
        reloading = true;
        _audioSource.clip = _reloadClip;
        _audioSource.Play();
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammo;
        reloading = false;
    }
    void OnGUI()
    {
        GUILayout.Label("AMMO: " + ammoCount);
    }
    private ProjectileColor ProjectileColorFromCubeColor(CubeColor cubeColor) => (ProjectileColor)cubeColor;
}
