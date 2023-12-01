using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private int _enemyScore = 10;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    [SerializeField]
    private Transform _laserSpawnPOS;
    [SerializeField]
    private int _enemySpeed;

    // Update is called once per frame
    void Start()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_anim == null)
        {
            Debug.LogError(" Animator is NULL");
        }
    }


      void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
            FireEnemyLaser();
    }
    
    private void FireEnemyLaser()
    {
        _fireRate = Random.Range(3.0f, 7.0f);
        _canFire = Time.time + _fireRate;

        Instantiate(_laserPrefab, _laserSpawnPOS.position, Quaternion.identity, transform);
        Laser[] lasers = GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }
    public void PowerBomb()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _enemySpeed = 0;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject);
    }

    void CalculateMovement ()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
        
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        // move down 4 meters per second
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        Player player = GameObject.Find("Player").GetComponent<Player>(); 
        if (player == null)
        {
            Debug.LogError("player script not found");
            return;
        }

        if (other.tag == "Player")
        {           
            player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject,2.8f);
        }
        
        if (other.tag == "Laser")
        {
            Laser laserScript = other.gameObject.GetComponent<Laser>();
            if (!laserScript)
            {
                Debug.LogError("Did NOT find laser script on object");
                return;
            }

            bool isFriendly = laserScript.GetIsEnemyLaser();
            
            if (isFriendly)
                return;

            Destroy(other.gameObject);

            player.AddScore(_enemyScore);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);        
        }
    }
   
}

