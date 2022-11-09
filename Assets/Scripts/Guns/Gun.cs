using System;
using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{ 
    [HideInInspector]
    public bool equipped;
    [Header("Gun References")]
    public Transform muzzle;
    public GameObject hole;
    public CrosshairData crosshairOverride;
    public AudioClip emptySfx;
    private AudioClip _shootSfx;
    //FX
    [Header("Properties")] 
    public float maxRange = 100f;
    public float damage = 2f;
    public bool automatic;
    public float fireRate = 17;
    private float _fireTimer;
    public int ammoCount = 12;
    public int maxAmmo = 12;
    [Header("Spread")]
    public float spreadSizePerShot = 0.2f;
    public float velocitySpreadMultiplier = 1f;
    public float spreadDecay = 0.2f;
    public float spreadMinSize = 0.1f;
    public float spreadMaxSize = 1.2f;
    private float _spread;

    private PlayerCamera _playerCamera;
    private VelocityController _velocityController;
    private AudioSource _audioSource;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private PlayerEquipment _playerEquipment;
    public event Action OnGunShoot = () => {};
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _shootSfx = _audioSource.clip;
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        equipped = false;
    }

    public void OnPickup()
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        _velocityController = GetComponentInParent<VelocityController>();
        _playerCamera = GetComponentInParent<PlayerCamera>();
        _playerEquipment = GetComponentInParent<PlayerEquipment>();
        if(_playerEquipment != null)
            _playerEquipment.UpdateAmmo($"{ammoCount}/{maxAmmo}");
        _spread = spreadMinSize;
        equipped = true;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop(Vector3 force)
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = force;
        if(_playerEquipment != null)
            _playerEquipment.UpdateAmmo("0/0");
        equipped = false;
    }

    private void Update()
    {
        if (!equipped) return;
        
        if (_spread > spreadMinSize)
        {
            _spread -= Time.deltaTime * spreadDecay;
        }

        if (_fireTimer > 0)
            _fireTimer -= Time.deltaTime;
        else
        {
            _spread += velocitySpreadMultiplier * Time.deltaTime * _velocityController.CurrentVelocity.magnitude;
            _spread = Mathf.Clamp(_spread, spreadMinSize, spreadMaxSize);
            
            if (((Mouse.current.leftButton.isPressed && automatic) ||
                 (Mouse.current.leftButton.wasPressedThisFrame && !automatic))
               )
            {
                if (ammoCount > 0)
                {
                    //Shoot
                    _fireTimer = 1f / fireRate;
                    ammoCount--;
                    if (_playerEquipment != null)
                        _playerEquipment.UpdateAmmo($"{ammoCount}/{maxAmmo}");
                    Vector3 direction = _playerCamera.Camera.forward;
                    Vector2 random = Random.insideUnitCircle * _spread / 10f;
                    direction += _playerCamera.Camera.right * random.x + _playerCamera.Camera.up * random.y;
                    if (Physics.Raycast(_playerCamera.Camera.position, direction, out var hit, maxRange))
                    {
                        var holeInstance = Instantiate(hole, hit.point, Quaternion.identity);
                        holeInstance.transform.forward = -hit.normal;
                        var healthComponent = hit.collider.GetComponentInParent<IHasHealth>();
                        if (healthComponent != null)
                        {
                            if (healthComponent.CriticalHitBox == hit.collider)
                                healthComponent.OnCriticalHit(damage);
                            if (healthComponent.RegularHitBox == hit.collider)
                                healthComponent.OnHit(damage);
                        }
                    }

                    OnGunShoot?.Invoke();
                    _audioSource.clip = _shootSfx;
                    _audioSource.Play();
                    _spread += spreadSizePerShot;
                    _spread = Mathf.Clamp(_spread, spreadMinSize, spreadMaxSize);
                }
                else
                {
                    _audioSource.clip = emptySfx;
                    _audioSource.Play();
                }
            }
        }
    }
}
