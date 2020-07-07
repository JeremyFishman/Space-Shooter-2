﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPowerup : MonoBehaviour
{
    [SerializeField]
    private float _Speed = 3.0f;
    [SerializeField]  //  0 = triple shot,  1 = speed,  2 = shields
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _Speed * Time.deltaTime);
        
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

            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                    player.TripleShotActive();
                    break;
                    case 1:
                    player.SpeedActive();
                    break;
                    case 2:
                    player.ShieldActive();
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
