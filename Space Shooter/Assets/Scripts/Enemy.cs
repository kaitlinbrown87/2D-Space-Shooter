using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private int _enemyScore = 10;

    private Player _player;

    private Animator _anim;
    private AudioSource _audioSource;
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
        // move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-8f, 8f);
          transform.position = new Vector3(randomX, 7, 0);
        }
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
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
            Destroy(other.gameObject);

            player.AddScore(_enemyScore);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject,2.8f);
        }

    }
}

