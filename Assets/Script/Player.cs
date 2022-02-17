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

    [SerializeField] private GameObject tankCanon;

    // Update is called once per frame
    void Update()
    {
        IsInput();
        MouseMove();
    }

    private void IsInput()
    {
        if (Input.GetAxis("Horizontal") < 0)
            transform.Rotate(0, -30 * Time.deltaTime, 0);


        if (Input.GetAxis("Horizontal") > 0)
            transform.Rotate(0, 30 * Time.deltaTime, 0);


        if (Input.GetAxis("Vertical") < 0)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (Input.GetAxis("Vertical") > 0)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    private void MouseMove()
    {
        //Gere le deplacement de la caméra avec la souris

        Y += Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);
        X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);

        tankCanon.transform.rotation = Quaternion.Euler(Mathf.Clamp(-Y, -90f, 90f), X, 0.0f);
        //transform.rotation = Quaternion.Euler(0, X, 0.0f);
    }

    private void Fire()
    {
        newBullet = Instantiate(obus, bulletExit.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletExit.forward * shootSpeed);
        
    }
}
