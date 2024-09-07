using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("GameLoop")]
    [SerializeField] private int _initialLives = 3;
    private int _currentLives;
    private int _currentScore;

    [Header("Asteroids")]
    [SerializeField] private List<GameObject> _asteroidSpawnLocations;
    [SerializeField] private Asteroid _asteroidPrefab;
    private List<Asteroid> _currentAsteroids = new();

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        var hiScore = 0; // TODO??
        _currentLives = _initialLives;
        _currentScore = 0;
        
        PlayerController.Instance.InitializePlayer();
        
        CanvasManager.Instance.UpdateLives(_currentLives);
        CanvasManager.Instance.UpdateCurrentScore(_currentScore);
        CanvasManager.Instance.UpdateHiScore(hiScore);

        // TODO: pool asteroids?
        foreach (var currentAsteroid in _currentAsteroids)
        {
            DestroyAsteroid(currentAsteroid, false);
        }
        _currentAsteroids.Clear();
        
        foreach (var asteroidSpawnLocation in _asteroidSpawnLocations)
        {
            SpawnAsteroid(asteroidSpawnLocation);
        }
    }

    private void SpawnAsteroid(GameObject asteroidSpawnLocation)
    {
        // TODO: pool asteroids?
        var newAsteroid = Instantiate(_asteroidPrefab);
        newAsteroid.transform.position = asteroidSpawnLocation.transform.position;
        WarpManager.Instance.SubscribeTransform(newAsteroid.transform);
        newAsteroid.InvokeInitialForce();
        _currentAsteroids.Add(newAsteroid);
    }

    public void OnBulletAsteroidCollision(Bullet bullet, Asteroid asteroid)
    {
        DestroyBullet(bullet);
        DestroyAsteroid(asteroid);

        _currentScore += asteroid.Score;
        CanvasManager.Instance.UpdateCurrentScore(_currentScore);
    }

    private static void DestroyBullet(Bullet bullet)
    {
        // TODO: pool bullets?
        Destroy(bullet.gameObject);
        WarpManager.Instance.UnsubscribeTransform(bullet.transform);
    }

    private void DestroyAsteroid(Asteroid asteroid, bool removeFromList = true)
    {
        // TODO: pool asteroids
        Destroy(asteroid.gameObject);
        WarpManager.Instance.UnsubscribeTransform(asteroid.transform);
        if (removeFromList)
            _currentAsteroids.Remove(asteroid);
    }

    public void OnAsteroidSpaceshipCollision(Spaceship spaceship, Asteroid asteroid)
    {
        CanvasManager.Instance.UpdateLives(_currentLives);
        spaceship.Rigidbody.HaltRigidbody();
        _currentLives--;
        
        if (_currentLives <= 0)
        {
            HandleGameOver();
        }
        else
        {
            //TODO: wait for like 5 seconds? 
            PlayerController.Instance.OnPlayerHit();
            CanvasManager.Instance.UpdateLives(_currentLives);
        }
    }

    private void HandleGameOver()
    {
        //TODO: wait for like 5 seconds? 
        StartGame();
    }
}