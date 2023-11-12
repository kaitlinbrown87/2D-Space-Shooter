using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private AudioClip _clip;
    
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
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            {
                switch (_powerupID)
                {
                    case 0:
                        player.trippleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive ();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;

                    default:
                        Debug.Log("Default Value");
                        break;
                    case 4:
                        player.RestoreLives();
                        break;
                    case 5:
                        player.PowerBomb();
                        break;

                }
            }
                

                Destroy(this.gameObject);
            }
        }




 }

 
