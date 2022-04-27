using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _obus;
    [SerializeField] private GameObject _mgBullet;

    [Header("Exit Bullets")]
    [SerializeField] private Transform _bulletExit;
    [SerializeField] private Transform _bulletMGexit;

    [Header("Instanciated GameObject")]
    private GameObject _newBullet;
    private GameObject _newMGBullet;

    [Header("Speeds")]
    private float _shootSpeed = 1000f;
    private float moveSpeed = 2;
    private float _speedRotate = 35;

    [Header("Needed Scene's GameObjects")]
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _tankCanon;
    [SerializeField] private GameObject _Door;

    [Header("Bools")]
    private bool _canShoot = true;
    private bool _canShootMG = true;
    
    [Header("UI")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject spriteCD;

    public int nbrOfTarget = 0;
    private Ray mouseRay;

    private void Start() //desactive l'UI
    {
        spriteCD.SetActive(false);
        winCanvas.SetActive(false);
    }

    void Update()
    {
        IsInput();
        MouseMove();
        CheckTargetLeft();
    }

    private void IsInput() //Gère les inputs
    {
        if (Input.GetAxis("Horizontal") < 0)    //rotation sur la gauche
            transform.Rotate(0, -30 * Time.deltaTime, 0);

        if (Input.GetAxis("Horizontal") > 0)    //rotation sur la droite
            transform.Rotate(0, 30 * Time.deltaTime, 0);

        if (Input.GetAxis("Vertical") < 0)      //Recule
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (Input.GetAxis("Vertical") > 0)      //Avance
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))   //Tire un Obus
        {
            if (_canShoot == true)
            {
                _canShoot = false;
                StartCoroutine(Fire());         //Tire et lance le cooldown de rechargement
            }
        }

        if (Input.GetKey(KeyCode.A))            //rotation camera sur la gauche
        {
            _cam.transform.parent.Rotate(0, -_speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.E))            //rotation camera sur la droite
        {
            _cam.transform.parent.Rotate(0, _speedRotate * Time.deltaTime, 0, Space.World);
        }

        if (Input.mouseScrollDelta.y > 0)       //Zoom au scroll de la souris vers le haut
        {
            _cam.transform.Translate(0, 0, _speedRotate * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y < 0)       //Dèzoom au scroll de la souris vers le bas
        {
            _cam.transform.Translate(0, 0, -_speedRotate * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))        //Tire a la machine gun
        {
            if (_canShootMG == true)
            {
                _canShootMG = false;
                StartCoroutine(FireMG());       //Tire et lance le cooldown de rechargement
            }
        }
    }

    private void MouseMove()                    //Tire un raycast par rapport a la position de la souris sur l'ecran
    {
        RaycastHit hit;
        mouseRay = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
        {
            if (hit.collider)
                _tankCanon.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));      //la tourelle du tank regarde le point d'inpact du raycast
        }

        Debug.DrawRay(_tankCanon.transform.position, mouseRay.direction * 10, Color.blue);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 10, Color.yellow);
    }

    IEnumerator Fire()      //Instancie une balle, lui ajoute une force et lance la coroutine de rechargement
    {
        _newBullet = Instantiate(_obus, _bulletExit.position, Quaternion.identity);
        _newBullet.GetComponent<Rigidbody>().AddForce(_bulletExit.forward * _shootSpeed);
        _newBullet.transform.parent = null;

        StartCoroutine(Reload());   //lance la coroutine de rechargement
        yield return null;
    }

    IEnumerator FireMG()    //Instancie une balle, lui ajoute une force et attend 0.2 sec entre chaque balles
    {
        _newMGBullet = Instantiate(_mgBullet, _bulletMGexit.position, Quaternion.identity);
        _newMGBullet.GetComponent<Rigidbody>().AddForce(_bulletMGexit.forward * _shootSpeed);
        _newMGBullet.transform.parent = null;

        yield return new WaitForSeconds(0.2f);
        _canShootMG = true;
    }

    IEnumerator Reload()        //active le sprite de cooldown et lance sont animation
    {
        spriteCD.gameObject.SetActive(true);
        spriteCD.GetComponent<Animator>().SetTrigger("begin");

        yield return new WaitForSeconds(3f);
        spriteCD.gameObject.SetActive(false);

        _canShoot = true;
    }

    private void CheckTargetLeft()      //Verifie combien de cible il reste, si target = 0 ouvre la porte
    {
        if(nbrOfTarget <= 0)
        {
            _Door.GetComponent<Animator>().SetTrigger("open");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("End"))      //si le joueur attein l'arrivé -> victoire
        {
            winCanvas.SetActive(true);
        }
    }
}
