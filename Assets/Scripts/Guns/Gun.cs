using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [Header("Gun References")]
    public Transform muzzle;
    public GameObject hole;
    //FX
    [Header("Properties")] 
    public float maxRange = 100f;
    public float damage = 2f;
    public bool automatic;
    public float fireRate = 17;
    private float _fireTimer;
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

    public event Action OnGunShoot = () => {};
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _velocityController = GetComponentInParent<VelocityController>();
        _playerCamera = GetComponentInParent<PlayerCamera>();
        _spread = spreadMinSize;
    }

    private void Update()
    {
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
            
            if ((Mouse.current.leftButton.isPressed && automatic) ||
                (Mouse.current.leftButton.wasPressedThisFrame && !automatic))
            {
                //Shoot
                _fireTimer = 1f / fireRate;
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
                        if(healthComponent.CriticalHitBox == hit.collider)
                            healthComponent.OnCriticalHit(damage);
                        if(healthComponent.RegularHitBox == hit.collider)
                            healthComponent.OnHit(damage);
                    }
                }
                OnGunShoot?.Invoke();
                _audioSource.Play();
                _spread += spreadSizePerShot;
                _spread = Mathf.Clamp(_spread, spreadMinSize, spreadMaxSize);
            }
        }
    }
}
