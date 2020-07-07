





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = .15f;

    [SerializeField]
    private float _canFire = -1f;

    public int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedActive = false;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _rightEngineVisualizer;

    [SerializeField]
    private GameObject _leftEngineVisualizer;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSound;

    [SerializeField]
    private AudioSource _audioSourceHandle;

    private bool _isInvincible = false;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSourceHandle = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        
        if (_audioSourceHandle == null)
        {
            Debug.LogError("The Audio Source is NULL");
        }
        else
        {
            _audioSourceHandle.clip = _laserSound;
        }

    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            fireLaser();
        }
        

        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }

    }
    
    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        

        if (_isSpeedActive == true)
        {
            _speed = 7.0f;
        }
        else
        {
            _speed = 3.5f;
        }

        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        
        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

    }
    void fireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSourceHandle.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        StartCoroutine(InvincibilityRoutine());

        
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngineVisualizer.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngineVisualizer.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        if (_isInvincible == false)
        {
            _lives -= 1;
            _isInvincible = true;
            yield return new WaitForSeconds(1.0f);
            _isInvincible = false;
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
