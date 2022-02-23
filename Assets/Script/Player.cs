using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 2;

    [SerializeField] private GameObject _obus;
    [SerializeField] private GameObject _mgBullet;
    [SerializeField] private Transform _bulletExit;
    private GameObject _newBullet;
    private GameObject _newMGBullet;
    [SerializeField] private Transform _bulletMGexit;
    private float _shootSpeed = 1000f;

    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _tankCanon;
    private float _speedRotate = 35;
    private bool _canShoot = true;
    private bool _canShootMG = true;
    [SerializeField] private GameObject _Door;
    [SerializeField] private int nbrOfTarget = 0;

    RaycastHit rayHit;
    Ray mouseRay;

    [SerializeField] private GameObject spriteCD;

    private void Start()
    {
        spriteCD.SetActive(false);
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
            if (_canShoot == true)
            {
                _canShoot = false;
                StartCoroutine(Fire());
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            _cam.transform.parent.Rotate(0, -_speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.E))
        {
            _cam.transform.parent.Rotate(0, _speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            _cam.transform.Translate(0, 0, _speedRotate * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _cam.transform.Translate(0, 0, -_speedRotate * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (_canShootMG == true)
            {
                _canShootMG = false;
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
        mouseRay = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
        {
            if (hit.collider)
                _tankCanon.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }

        Debug.DrawRay(_tankCanon.transform.position, mouseRay.direction * 10, Color.blue);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 10, Color.yellow);
    }

    IEnumerator Fire()
    {
        _newBullet = Instantiate(_obus, _bulletExit.position, Quaternion.identity);
        _newBullet.GetComponent<Rigidbody>().AddForce(_bulletExit.forward * _shootSpeed);
        _newBullet.transform.parent = null;

        StartCoroutine(Reload());

        yield return null;
    }

    IEnumerator FireMG()
    {
        _newMGBullet = Instantiate(_mgBullet, _bulletMGexit.position, Quaternion.identity);
        _newMGBullet.GetComponent<Rigidbody>().AddForce(_bulletMGexit.forward * _shootSpeed);
        _newMGBullet.transform.parent = null;

        yield return new WaitForSeconds(0.2f);

        _canShootMG = true;
    }

    IEnumerator Reload()
    {
        spriteCD.gameObject.SetActive(true);
        spriteCD.GetComponent<Animator>().SetTrigger("begin");

        yield return new WaitForSeconds(3f);
        spriteCD.gameObject.SetActive(false);

        _canShoot = true;
    }
}
