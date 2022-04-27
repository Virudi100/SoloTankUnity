using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)      //si il rentre en collision avec le joueur, detruit le joueur et ce detruit lui meme
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
