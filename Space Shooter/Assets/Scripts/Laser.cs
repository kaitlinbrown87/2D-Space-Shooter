using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update

    // Speed Variable of 8
    private float _speed = 8.0f;
        // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up *_speed* Time.deltaTime);
       
        if(transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
