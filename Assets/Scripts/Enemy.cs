using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    
    private Player _player;
    
    private Animator _animatorHandle;

    [SerializeField]
    private AudioClip _explosionSound;

    [SerializeField]
    private AudioSource _audioSourceHandle;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 3.0f;

    [SerializeField]
    private float _canFire = -1.0f;

    private bool _isEnemyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSourceHandle = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        
        _animatorHandle = GetComponent<Animator>();

        if (_animatorHandle == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        if (_audioSourceHandle == null)
        {
            Debug.LogError("The Audio Sorce is NULL");
        }
        else
        {
            _audioSourceHandle.clip = _explosionSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && transform.position.y > 0  && _isEnemyDead == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = (Time.time + _fireRate);
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i+=1)
            {
                lasers[i].AssignEnemyLaser();
            }

        }

        if (_player._lives < 1)
        {
            Destroy(this.gameObject);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _animatorHandle.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSourceHandle.Play();
            Destroy(GetComponent<Collider2D>());
            _isEnemyDead = true;
            Destroy(this.gameObject, 2.8f);
        }
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _player.AddScore(10);
            }
            _animatorHandle.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSourceHandle.Play();
            Destroy(GetComponent<Collider2D>());
            _isEnemyDead = true;
            Destroy(this.gameObject, 2.8f);
        }

    }
}
