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
    [SerializeField]
    private int _powerBombShot;
    private bool _isPowerBombActive = false;
    [SerializeField]
    private AudioClip _powerUpSoundClip;
    [SerializeField]
    private GameObject[] _enemiesArray;
    [SerializeField]
    private float _powerUpThrustersWaitTimeLimit = 3.0f;
    [SerializeField]
    private float _thrusterChargeLevelMax = 10.0f;
    [SerializeField]
    private float _thrusterChargeLevel;
    [SerializeField]
    private float _changeDecreaseThrusterChargeBy = 1.5f;
    [SerializeField]
    private float _changeIncreaseThrusterChargeBy = 0.01f;
    private bool _canUseThruster = true;
    private bool _thrusterInUse = false;
    [SerializeField]
    public float _shakeDuration = 0.4f;
    [SerializeField]
    public float _shakeMagnitude = 0.5f;
    [SerializeField]
    CameraShake _cameraShake;
    private bool _isSlowDownActive = false;
    

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

        if (_uIManager == null)
        {
            Debug.LogError("UI manager is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("audio source from player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
        _cameraShake = GameObject.Find("CameraShake").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        void ThrusterChargeLevel ()
        {
            _thrusterChargeLevel = Mathf.Clamp(_thrusterChargeLevel, 0, _thrusterChargeLevelMax);
            if (_thrusterChargeLevel <=0.0f)
            {
                _canUseThruster = false;
            }
            else if (_thrusterChargeLevel >= (_thrusterChargeLevelMax/0.75f))
            {
                _canUseThruster = true;
            }
        }
        void ThrusterAcceleration()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)&& _canUseThruster)
            {
                _speed *= _speedMultiplier;
                _thrusterInUse = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _speed = 3.5f;
                _thrusterInUse = false;
            }
            if (_thrusterInUse)
            {
                ThrusterActive();
            }
            else if (!_thrusterInUse)
            {
                StartCoroutine(ThrusterRepinishRoutine());
            }
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
        if (_isSlowDownActive)
        {
            _speed = 5.0f;
            _speed = _speed / 2;
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
        _ammoCount--;


        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject newLaser = Instantiate(_LaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
        if (_isPowerBombActive && _powerBombShot >0)
        {
            PowerBombEngage();
            _powerBombShot--;
        }

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
            StartCoroutine(_cameraShake.Shake(_shakeDuration, _shakeMagnitude));
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
    public void AddAmmo ()
    {
        _ammoCount += 15;
        _uIManager.ChangeAmmoCount(_ammoCount);
    }
    public void RestoreLives ()
    {
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 3)
            {
                _leftEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _rightEngine.SetActive(false);
            }
            _uIManager.Updatelives(_lives);
        }
    }
    public void PowerBomb ()
    {
        _isPowerBombActive = true;
        _powerBombShot = 1;
        _audioSource.PlayOneShot(_powerUpSoundClip);
        StartCoroutine(PowerBombPowerDownRoutine());
    }
    IEnumerator PowerBombPowerDownRoutine ()
    {
        yield return new WaitForSeconds(5.0f);
        _isPowerBombActive = true;
    }
    public void PowerBombEngage()
    {
        _enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in _enemiesArray)
        {
            enemy.GetComponent<Enemy>().PowerBomb();
        }
    }
    public void ThrusterActive()
    {
        if (_canUseThruster == true)
        {
            _thrusterChargeLevel -= Time.deltaTime * _changeDecreaseThrusterChargeBy;
            _uIManager.UpdateThrusterSlider(_thrusterChargeLevel);
        }
        if (_thrusterChargeLevel <= 0)
        {
            _uIManager.ThrusterSliderUsableCoolor(false);
            _thrusterInUse = false;
            _canUseThruster = false;
            _speed = 0.0f;
        }
    }
    IEnumerator ThrusterRepinishRoutine ()
    {
        yield return new WaitForSeconds(_powerUpThrustersWaitTimeLimit);
        while (_thrusterChargeLevel <= _thrusterChargeLevel && !_thrusterInUse) ;
        {
            yield return null;
            _thrusterChargeLevel += Time.deltaTime * _changeIncreaseThrusterChargeBy;
            _uIManager.UpdateThrusterSlider(_thrusterChargeLevel);
        }
        if (_thrusterChargeLevel >= _thrusterChargeLevelMax)
        {
            _uIManager.ThrusterSliderUsableCoolor(true);
            _canUseThruster = true;
        }
    }
     void SlowDownPowerUpActive()
    {
        _isSlowDownActive = true;
        StartCoroutine(SlowDownTurnOffRoutine());
    }
    IEnumerator SlowDownTurnOffRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSlowDownActive = false;
    }
}
        

    