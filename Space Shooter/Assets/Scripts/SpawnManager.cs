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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator -- Yield events
    // while loop
    IEnumerator SpawnRoutine ()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }

        // then this line is called 
        // while loop (infinate loop)
        // instantiate enemy prefab
        // yield for 5 seconds

    }
    public void _OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    
}
