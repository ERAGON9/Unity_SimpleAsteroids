using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Spaceship")]
    [SerializeField] private Spaceship _spaceshipPrefab;
    [SerializeField] private float m_TrustForce;
    [SerializeField] private float m_RotationDegrees = 10;
    [SerializeField] private Vector3 _initialPosition;
    
    [Header("Bullets")]
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_ShootingForce;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_LaserClip;
    
    
    private Spaceship _spaceship;

    private void Awake()
    {
        _spaceship = Instantiate(_spaceshipPrefab);
        WarpManager.Instance.SubscribeTransform(_spaceship.transform);
    }
    
    public void InitializePlayer()
    {
        _spaceship.transform.position = _initialPosition;
    }

    public void OnPlayerHit()
    {
        // TODO: animate and limit invincibility and input
        InitializePlayer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleTrust();
        handleRotation();
        handleBullets();
    }

    private void handleBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: Implement cooldown
            // TODO: Implement object pooling
            SpawnBullet();
            m_AudioSource.PlayOneShot(m_LaserClip);
        }
    }

    private void SpawnBullet()
    {
        var bullet = Instantiate(m_BulletPrefab);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.transform.position = _spaceship.transform.position;
        bulletRigidbody.AddForce(_spaceship.transform.up * m_ShootingForce, ForceMode2D.Impulse);
        WarpManager.Instance.SubscribeTransform(bulletRigidbody.transform);
    }

    private void handleTrust()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var spaceShipTransform = _spaceship.transform;
            var forceVector = spaceShipTransform.up * (m_TrustForce * Time.deltaTime);
            _spaceship.Rigidbody.AddForce(forceVector, ForceMode2D.Force);
        }
    }
    
    private void handleRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _spaceship.transform.Rotate(Vector3.forward, -1 * m_RotationDegrees * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _spaceship.transform.Rotate(Vector3.forward, m_RotationDegrees * Time.deltaTime);
        }
    }
    
    protected override void OnDestroy()
    {
        WarpManager.Instance.UnsubscribeTransform(_spaceship.transform);
        base.OnDestroy();
    }
}
