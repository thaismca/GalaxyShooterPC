using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player (Ship)
    [SerializeField]
    private float _speed = 8.0f;
    public int lives = 3;
    [SerializeField]
    private GameObject _playerExplosionPrefab;
    public bool _isPlayer1 = false;
    public bool _isPlayer2 = false;

    //Powerups states
    private bool _powerUpTripleShot = false;
    private int _tripleShotTime = 0;
    private bool _powerUpSpeedBoost = false;
    private int _speedBoostTime = 0;
    private bool _powerUpShield = false;
    private int _shieldTime = 0;

    //Laser
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    //Laser cool down system
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;
    //Audio
    private AudioSource _audioSource;

    //Shield
    [SerializeField]
    private GameObject _shieldGameObject;

    //Engine Failure
    [SerializeField]
    private GameObject _firstDamage;
    [SerializeField]
    private GameObject _secondDamage;

    //UIManager
    private UIManager _uiManager;
    //GameManager
    private GameManager _gameManager;
    //SpawnManager
    private SpawnManager _spawnManager;


    // Use this for initialization
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayer1)
        {
            //enables my player to move
            MovementPlayer1();

            //fires a laser whenever the player presses space or click the mouse left button
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
        }

        if (_isPlayer2)
        {
            //enables my player to move
            MovementPlayer2();

            //fires a laser whenever the player presses space or click the mouse left button
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Fire();
            }
        }
    }

    //Custom methods

    //Lives System
    public void Damage()
    {
        //check if there's a shield enabled - if so, disable it
        if (_powerUpShield)
        {
            _powerUpShield = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        lives--;
        
        //Update the UI for lives display
        if (_isPlayer1)
        {
            _uiManager.UpdatePlayer1Lives(lives);
        }
        else if (_isPlayer2)
        {
            _uiManager.UpdatePlayer2Lives(lives);
        }

        //If it's player's last life, kill the Player
        if (lives == 2)
        {
            _firstDamage.SetActive(true);
        }
        else if (lives == 1)
        {
            _secondDamage.SetActive(true);
        }
        else if (lives < 1)
        {
            Instantiate(_playerExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            _gameManager.deadPlayer++;
        }

        if(_gameManager.isCoopMode == false && _gameManager.deadPlayer == 1)
        {
            _gameManager.GameOver();
        }
        else if (_gameManager.isCoopMode == true && _gameManager.deadPlayer == 2)
        {
            _gameManager.GameOver();
        }
    }


    //Moving the Player 1
    private void MovementPlayer1()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Moving 1.5x faster than normal speed (Speed Up Power Up)
        if (_powerUpSpeedBoost)
        {
            transform.Translate(Vector3.right * (_speed * 1.5f) * horizontalInput * Time.deltaTime); //move left-right 1.5x faster
            transform.Translate(Vector3.up * (_speed * 1.5f) * verticalInput * Time.deltaTime); //move up-down 1.5x faster
        }

        //Moving at normal speed
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime); //move left-right
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime); //move up-down
        }

        Wraping();
    }


    //Moving the Player 2
    private void MovementPlayer2()
    {
        if (_powerUpSpeedBoost)
        {
            //Move left
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * (_speed * 1.5f) * Time.deltaTime);
            }

            //Move right
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * (_speed * 1.5f) * Time.deltaTime);
            }

            //Move up
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * (_speed * 1.5f) * Time.deltaTime);
            }

            //Move down
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * (_speed * 1.5f) * Time.deltaTime);
            }
        }
        else
        {
            //Move left
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * _speed* Time.deltaTime);
            }

            //Move right
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right *_speed * Time.deltaTime);
            }

            //Move up
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }

            //Move down
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }

        Wraping();
    }


    //Wraping
    void Wraping()
    {
        //On the y axis
        if (transform.position.y <= -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }
        else if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        //On the x axis
        if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
        else if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
    }


    //Firing Laser
    private void Fire()
    {
        if (Time.time > _canFire)
        {
            //play firing Sound FX
            _audioSource.Play();
            
            //Firing Triple Shot 
            if (_powerUpTripleShot)
            {
                //spaw laser from right above the player position
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            //Firing normal single shot
            else
            {
                //spaw laser from right above the player position
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }

            //implementing the cool down system
            _canFire = Time.time + _fireRate;
        }
    }


    //Enable Triple Shot Powerup
    public void TripleShotPowerupOn()
    {
        _powerUpTripleShot = true;
        StopCoroutine("TripleShotPowerDownRoutine");
        _tripleShotTime += 5;
        StartCoroutine("TripleShotPowerDownRoutine");
    }
    
    //Disable Triple Shot Powerup
    public IEnumerator TripleShotPowerDownRoutine()
    {
        int temp = _tripleShotTime;
        for (int i = 0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_tripleShotTime;
        }
        _powerUpTripleShot = false;
    }


    //Enable Speed Boost Powerup
    public void SpeedUpPowerupOn()
    {
        _powerUpSpeedBoost = true;
        StopCoroutine("SpeedBoostPowerDownRoutine");
        _speedBoostTime += 5;
        StartCoroutine("SpeedBoostPowerDownRoutine");
    }

    //Disable Speed Boost Powerup
    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        int temp = _speedBoostTime;
        for (int i=0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_speedBoostTime;
        }
        _powerUpSpeedBoost = false;
    }


    //Enable Shield Power Up
    public void ShieldPowerupOn()
    {
        _powerUpShield = true;
        StopCoroutine("ShieldPowerDownRoutine");
        _shieldGameObject.SetActive(true);
        _shieldTime += 10;
        StartCoroutine("ShieldPowerDownRoutine");
    }

    //Disable Shield Power Up after 10secs
    public IEnumerator ShieldPowerDownRoutine()
    {
        int temp = _shieldTime;
        for (int i=0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_shieldTime;
        }
        _powerUpShield = false;
        _shieldGameObject.SetActive(false);
    }
}