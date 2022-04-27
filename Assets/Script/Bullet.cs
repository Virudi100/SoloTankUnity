using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LifeTime());         //lance la coroutine
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("turret"))  //si il rentre en contact avec une tourelle, il la detruit et ce detruit lui meme
        {
            _player.GetComponent<Player>().nbrOfTarget--;
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    IEnumerator LifeTime() //Detruit le projectile apres 1sec
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
