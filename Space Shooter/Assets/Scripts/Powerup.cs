using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
   [SerializeField]
    private float _speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3 (adjust in inspector)
        // when we leave this screen, destroy objecct 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // communicate with Player script
            // handle to the component I want
            // assigin handle to the component
            Player player = other.transform.GetComponent<Player>();
            if (player != null )
            {
                player.trippleShotActive();
            }
            Destroy(this.gameObject);
        }
    }


    // OnTriggerCollision 
    // Only be collectable by Player (HINT: Use tags)
    // on collected destroy 
}
