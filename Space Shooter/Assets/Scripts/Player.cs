using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    public float horizontalinput;
    public float verticalinput;
    public float BottomLimit = -3.8f;
    public float RightLimit = 11.3f;
    public float LeftLimit = -11.3f;
    [SerializeField]
    public GameObject _LaserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTriggerShotActive = false;

    

    void Start()
    {
        // take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager ==null)
        {
            Debug.LogError("Spawn manager is NULL.");
        }
        //Find the object. Get the component
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

       


        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y <= BottomLimit)
        {
            transform.position = new Vector3(transform.position.x, BottomLimit, 0f);
        }

        if (transform.position.x > RightLimit)
        {
            transform.position = new Vector3(LeftLimit, transform.position.y, 0f);
        }
        else if (transform.position.x < LeftLimit)
        {
            transform.position = new Vector3(RightLimit, transform.position.y, 0f);
        }
    }

    void FireLaser()
    {
     
            _canFire = Time.time + _fireRate;
        if (_isTriggerShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_LaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            
        }

        


        // instantiate 3 lasers (triple shot prefab)

    }
    public void Damage()
    {
        _lives -= 1;
       
        if (_lives < 1)
        {
            _spawnManager._OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }
    public void trippleShotActive()
    {
        _isTriggerShotActive = true;
        StartCoroutine (TripleShotPowerDownRoutine());
    } 
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTriggerShotActive = false;
    }
}
    