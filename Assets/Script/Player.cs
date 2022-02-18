using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 2;

    [SerializeField] private GameObject obus;
    [SerializeField] private Transform bulletExit;
    private GameObject newBullet;
    private float shootSpeed = 1000f;

    private float sensitivity = 100;
    private float X;
    private float Y;

    [SerializeField] private Camera cam;

    [SerializeField] private GameObject tankCanon;

    private float speedRotate = 35;

    RaycastHit rayHit;
    Ray ray;

    // Update is called once per frame
    void Update()
    {
        IsInput();
        MouseMove();

    }

    private void IsInput()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray.origin, ray.direction, out rayHit);

        if (Input.GetAxis("Horizontal") < 0)
            transform.Rotate(0, -30 * Time.deltaTime, 0);

        if (Input.GetAxis("Horizontal") > 0)
            transform.Rotate(0, 30 * Time.deltaTime, 0);

        if (Input.GetAxis("Vertical") < 0)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (Input.GetAxis("Vertical") > 0)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //tankCanon.transform.rotation = Quaternion.LookRotation(new Vector3(rayHit.point.x, 0, -rayHit.point.z));
            Fire();
        }

        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.parent.Rotate(0, -speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.E))
        {
            cam.transform.parent.Rotate(0, speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            cam.transform.Translate(0, 0, speedRotate * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            cam.transform.Translate(0, 0, -speedRotate * Time.deltaTime);
        }
    }

    private void MouseMove()
    {
        /*
        //Gere le deplacement de la caméra avec la souris

        Y += Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);
        X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);

        */

        tankCanon.transform.rotation = Quaternion.LookRotation(new Vector3(ray.direction.x, 0, ray.direction.z));

        Debug.DrawRay(tankCanon.transform.position, ray.direction * 10, Color.blue);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
    }

    private void Fire()
    {
        newBullet = Instantiate(obus, bulletExit.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletExit.forward * shootSpeed);
    }
}


/* Faire apparaitre le raycast du tank au moment du tire car tu sais ou le tire doit aller*/
