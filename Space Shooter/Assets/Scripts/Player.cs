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
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;


    void Start()
    {
        // take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        CalculateMovement();        
    }

    void CalculateMovement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)&& Time.time >_canFire)
        {
            FireLaser();
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
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }
    public void Damage()
    {
        _lives -= 1;
       
        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
    