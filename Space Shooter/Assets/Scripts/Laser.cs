using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update

    // Speed Variable of 8
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            //Check and see if this object has a parent 
            // Destroy the parent too 
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public bool GetIsEnemyLaser()
    {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player" && _isEnemyLaser== true)
        {
            Player player = other.GetComponent<Player>();
            if (player!= null)
            {
                player.Damage();
            }
        }        
    }
}
   