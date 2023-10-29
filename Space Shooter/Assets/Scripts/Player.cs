using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    public float horizontalinput;
    public float verticalinput;
    public float BottomLimit = -3.8f;
    public float RightLimit = 11.3f;
    public float LeftLimit = -11.3f;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private int _score;
    private UIManager _uIManager;
   [SerializeField]
   private AudioClip _laserSoundClip;
   
    private AudioSource _audioSource;
    [SerializeField]
    private float _thrusterSpeed = 4.0f;
    [SerializeField]
    private float _shieldHealth = 3f;
    [SerializeField]
    private SpriteRenderer _shieldColor;
    [SerializeField]
    private int _ammoCount = 15;

    // variable to store audio clip
    void Start()
    {

      
        
        
        
        // take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL.");
        }

        if (_uIManager == null )
        {
            Debug.LogError("UI manager is null");
        }
        
        if (_audioSource== null)
        {
            Debug.LogError("audio source from player is NULL");
        }
        else 
        {
            _audioSource.clip = _laserSoundClip;
        }
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * _speed * _thrusterSpeed * Time.deltaTime);
        }

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
        if (_ammoCount <= 0) return;
        {
            _ammoCount -= 1;
            _uIManager.ChangeAmmoCount(_ammoCount); 
        }
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject newLaser = Instantiate(_LaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();

        //play the laser audio clip
    }
    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldHealth--;
            if (_shieldHealth == 2)
            {
                _shieldColor.color = Color.yellow;
                return;
            }
            else if (_shieldHealth == 1)
            {
                _shieldColor.color = Color.red;
                return;
            }
            else if (_shieldHealth <= 0)
            {
                _shieldVisualizer.SetActive(false);
                _isShieldActive = false;
                return;
            }
        }
        else
        {
            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }




            _lives -= 1;
            _uIManager.Updatelives(_lives);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }

        

    }
    public void trippleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);
        Debug.Log("activating shields");
    }

    public void AddScore(int amount)
    {
        _score += amount;
        _uIManager.UpdateScore(_score);
    }
    

    // Method to add 10 to the score
    // communicate with the UI to update score
}


    