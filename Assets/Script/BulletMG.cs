using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMG : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("turret"))
        {
            collision.gameObject.GetComponent<Turret>().HP--;
        }
        Destroy(gameObject);
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
