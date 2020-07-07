using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 9.0f;
    
    [SerializeField]
    private GameObject _asteroidExplosionPrefab;
    
    [SerializeField]
    private SpawnManager _spawnManagerHandle;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManagerHandle = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate ateroid on the zed axis
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_asteroidExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManagerHandle.StartSpawning();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, .25f);
        }
    }
}
