using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 19.0f;
    [SerializeField]
    private GameObject _explodePrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explodePrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            int waveNumber = 1;
            _uiManager.DisplayWaveNumber(waveNumber);
            _spawnManager.StartSpawning(waveNumber);
            Destroy(this.gameObject, 0.25f);
            
        }
        
       
       
    }









    // check for LASER collision (trigger)
    // Instantiate explosion at position of asteroid 
    // Destroy explosion after 3 seconds.
}
