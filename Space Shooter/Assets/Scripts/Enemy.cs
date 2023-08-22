using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private int _enemyScore = 10;

    // Update is called once per frame
    void Start()
    {
        
    }


      void Update()
    {
        // move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if bottom of the screen 
        // respawn at top with random x position

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
            Destroy(this.gameObject);
        }
        
        if (other.tag == "Laser")
        {            
            Destroy(other.gameObject);

            player.AddScore(_enemyScore);

            Destroy(this.gameObject);
        }

    }
}

