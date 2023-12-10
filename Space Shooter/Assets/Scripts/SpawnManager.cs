using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerups;
    private int _waveNumber;
    private int _enemiesDead;
    private int _maxEnemies;
    private int _enemmiesLeftToSpawn;
    private UIManager _uiManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning(int waveNumber)
    {
        if (waveNumber <= 5)
        {
            _stopSpawning = false;
            _enemiesDead = 0;
            _waveNumber = waveNumber;
            _uiManager.DisplayWaveNumber(_waveNumber);
            _enemmiesLeftToSpawn = _waveNumber + 5;
            _maxEnemies = _waveNumber + 3;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
        }
    }
        IEnumerator SpawnEnemyRoutine ()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemmiesLeftToSpawn--;
            if (_enemmiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            StartSpawning(_waveNumber + 1);
        }

    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            if (randomPowerUp ==6)
            {
                randomPowerUp = Random.Range(0, 6);
            }
            Instantiate (powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
  public void EnemyDead ()
    {
        _enemiesDead++;
    }
}


