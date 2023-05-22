using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [SerializeField]
    private float _speed = 3.5f;
    public float horizontalinput;
    public float verticalinput;


    void Start()
    {
        // take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    //   Update is called once per frame
    void Update()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");

        //     new vector3(-1,0,0)0*5*3.5*real time
        //  transform.Translate(Vector3.right*horizontalinput *_speed*Time.deltaTime);
        //  transform.Translate(Vector3.up * verticalinput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // if player position on the Y is greater than 0
        // Y position = 0

        if (transform.position.y >= 0);
        {

            transform.position = new Vector3(transform.position.x, 0, 0);
        }
    }
} 
