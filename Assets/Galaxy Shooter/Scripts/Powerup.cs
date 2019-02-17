using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    private float _speed = 3.0f;
    [SerializeField]
    private int index;
    [SerializeField]
    private AudioClip _clip;

    //Use this for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        
        //move down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Destroy the object when it gets completely out of the canvas
        if (transform.position.y <= -5.5f)
        {
            Destroy(this.gameObject);
        }

    }

    //Collision detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if the powerup collided with the player
        if (other.tag == "Player")
        {
            //access the player
            Player p = other.GetComponent<Player>();

            if (p != null)
            {
                if (this.index == 0)
                {
                    p.TripleShotPowerupOn();
                }
                else if (this.index == 1)
                {
                    p.SpeedUpPowerupOn();
                }
                else if (this.index == 2)
                {
                    p.ShieldPowerupOn();
                }

            }

            //play powerup pickup SoundFX
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
            //destroy the object
            Destroy(this.gameObject);
        }

    }

}
