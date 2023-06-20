using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ [SerializeField]
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
    // Spawn a game object every 5 seconds.
    // Create a coroutine of type IEnumerator -- yield events
    // While loop
    IEnumerator SpawnRoutine()
    {
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
           GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
        // WE NEVER GET TO HERE 
    }
    public void OnPlayerDeath ()
    {
        _stopSpawning = true;
    }
}

