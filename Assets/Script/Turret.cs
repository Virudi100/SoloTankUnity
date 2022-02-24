using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] private GameObject player;


    [SerializeField] private GameObject obus;
    [SerializeField] private Transform bulletExit;
    private GameObject newBullet;
    private float shootSpeed = 300f;
    private bool canShoot = true;
    [SerializeField] private Transform canonTurret;
    private int detectionIndex = 15;

    private RaycastHit rayHit;

    // Update is called once per frame
    void Update()
    {
        IsTankDetected();
    }

    private void IsTankDetected()
    {
        if (player.gameObject != null)
        {
            Vector3 direction = Vector3.Normalize(player.transform.position - canonTurret.position);

            if (Physics.Raycast(canonTurret.position, direction, out rayHit, detectionIndex))
            {
                if (rayHit.collider.gameObject.CompareTag("Player"))
                {
                    {
                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z) * Time.deltaTime);

                        if (canShoot == true)
                        {
                            canShoot = false;
                            StartCoroutine(Fire());
                        }
                    }
                }
                Debug.DrawRay(canonTurret.position, direction, Color.green);
            }
        }
    }

    IEnumerator Fire()
    {
        newBullet = Instantiate(obus, bulletExit.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletExit.forward * shootSpeed);
        newBullet.transform.parent = null;

        yield return new WaitForSeconds(1);
        canShoot = true;
    }
}
