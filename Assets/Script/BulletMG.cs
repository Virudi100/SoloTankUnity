using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMG : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(LifeTime());  //Lance coroutine
    }

    private void OnCollisionEnter(Collision collision) //si il rentre en collision avec une tourelle, lui enleve 1 HP et ce détruit lui meme
    {
        if (collision.gameObject.CompareTag("turret"))
        {
            collision.gameObject.GetComponent<Turret>().HP--;
        }
        Destroy(gameObject);
    }

    IEnumerator LifeTime()   //S'autodetruit apres 1sec
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
