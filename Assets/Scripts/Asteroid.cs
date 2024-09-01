using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_Rigidbody;
    public Rigidbody2D Rigidbody => m_Rigidbody;

    [SerializeField] private float m_InitialThrustForce;
    [SerializeField] private int m_Score;
    public int Score => m_Score;

    public void InvokeInitialForce()
    {
        m_Rigidbody.AddForce(Random.insideUnitCircle.normalized * m_InitialThrustForce, ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.OnBulletAsteroidCollision(other.GetComponent<Bullet>(), this); 
        }        

        if (other.CompareTag("Ship"))
        {
            GameManager.Instance.OnAsteroidSpaceshipCollision(other.GetComponent<Spaceship>(), this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
