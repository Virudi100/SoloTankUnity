using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("turret"))
        {
            _player.GetComponent<Player>().nbrOfTarget--;
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
