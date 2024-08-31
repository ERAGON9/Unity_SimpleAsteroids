using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Spaceship")]
    [SerializeField] private Rigidbody2D m_SpaceshipRigidbody;
    [SerializeField] private float m_TrustForce;
    [SerializeField] private float m_RotationDegrees = 10;
    
    [Header("Bullets")]
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_ShootingForce;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_LaserClip;

    
    
    private float m_RotationDelta;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WarpManager.Instance.KeepInBounds(m_SpaceshipRigidbody.transform);
        handleTrust();
        handleRotation();
        handleBullets();
    }

    private void handleBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(m_BulletPrefab);
            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.transform.position = m_SpaceshipRigidbody.transform.position;
            bulletRigidbody.AddForce(m_SpaceshipRigidbody.transform.up * m_ShootingForce, ForceMode2D.Impulse);
            m_AudioSource.PlayOneShot(m_LaserClip);
        }
    }

    private void FixedUpdate()
    {
        m_SpaceshipRigidbody.MoveRotation(m_SpaceshipRigidbody.rotation + m_RotationDelta);
    }

    private void handleTrust()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var spaceShipTransform = m_SpaceshipRigidbody.transform;
            var forceVector = spaceShipTransform.up * (m_TrustForce * Time.deltaTime);
            m_SpaceshipRigidbody.AddForce(forceVector, ForceMode2D.Force);
        }
    }
    
    private void handleRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_RotationDelta = -1 * m_RotationDegrees * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_RotationDelta = m_RotationDegrees * Time.deltaTime;
        }
        else
        {
            m_RotationDelta = 0;
        }
    }
}
