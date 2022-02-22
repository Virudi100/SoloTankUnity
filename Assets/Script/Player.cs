using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 2;

    [SerializeField] private GameObject obus;
    [SerializeField] private GameObject mgBullet;
    [SerializeField] private Transform bulletExit;
    private GameObject newBullet;
    private GameObject newMGBullet;
    [SerializeField] private Transform bulletMGexit;
    private float shootSpeed = 1000f;

    /*private float sensitivity = 100;
    private float X;
    private float Y;*/

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject tankCanon;
    private float speedRotate = 35;
    private bool canShoot = true;
    private bool canShootMG = true;

    RaycastHit rayHit;
    Ray mouseRay;


    [Header("Sprite Reload")]
    [SerializeField] private GameObject sprite3sec;
    [SerializeField] private GameObject sprite2sec;
    [SerializeField] private GameObject sprite1sec;


    private float maxDistance;
    private void Start()
    {
        sprite3sec.SetActive(false);
        sprite2sec.SetActive(false);
        sprite1sec.SetActive(false);
    }

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canShoot == true)
            {
                canShoot = false;
                StartCoroutine(Fire());
            }
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

        if(Input.GetKey(KeyCode.Space))
        {
            if(canShootMG == true)
            {
                canShootMG = false;
                StartCoroutine(FireMG());
            }
            
        }
    }

    private void MouseMove()
    {
        /*
        //Gere le deplacement de la caméra avec la souris

        Y += Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);
        X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);

        */
        RaycastHit hit;
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
        {
            if(hit.collider)
                tankCanon.transform.LookAt(new Vector3(hit.point.x,0,hit.point.z));
        }
        
        Debug.DrawRay(tankCanon.transform.position, mouseRay.direction * 10, Color.blue);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 10, Color.yellow);
    }

    IEnumerator Fire()
    {
        newBullet = Instantiate(obus, bulletExit.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletExit.forward * shootSpeed);
        newBullet.transform.parent = null;

        StartCoroutine(Reload());

        yield return null;
        
    }

    IEnumerator FireMG()
    {
        newMGBullet = Instantiate(mgBullet, bulletMGexit.position, Quaternion.identity);
        newMGBullet.GetComponent<Rigidbody>().AddForce(bulletMGexit.forward * shootSpeed);
        newMGBullet.transform.parent = null;
        
        yield return new WaitForSeconds(0.2f);
        
        canShootMG = true;
    }

    IEnumerator Reload()
    {
        int i = 4;

        while(i > 0)
        {
            i--;

            yield return new WaitForSeconds(1f);

            if (i == 3)
            {
                sprite3sec.SetActive(true);
                sprite2sec.SetActive(false);
                sprite1sec.SetActive(false);
            }
            else if( i ==2)
            {
                sprite3sec.SetActive(false);
                sprite2sec.SetActive(true);
                sprite1sec.SetActive(false);
            }
            else if(i ==1)
            {
                sprite3sec.SetActive(false);
                sprite2sec.SetActive(false);
                sprite1sec.SetActive(true);
            }
            else
            {
                sprite3sec.SetActive(false);
                sprite2sec.SetActive(false);
                sprite1sec.SetActive(false);

                canShoot = true;
            }
        }   
    }
}


/* Faire apparaitre le raycast du tank au moment du tire car tu sais ou le tire doit aller*/
