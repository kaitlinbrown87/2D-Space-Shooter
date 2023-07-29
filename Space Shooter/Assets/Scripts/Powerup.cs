using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] // 0 = Triple Shot 1 = Speed 2 = Shield
    private int _powerupID;
    // ID for powerups 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.trippleShotActive();
                        break;
                    case 1:
                        Debug.Log("Collected Speed Boost");
                        break;
                    case 2:
                        Debug.Log("Collected Sheild");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }
            }
                

                Destroy(this.gameObject);
            }
        }




 }

 
