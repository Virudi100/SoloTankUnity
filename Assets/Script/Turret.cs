using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Shoot System")]
    [SerializeField] private GameObject obus;
    [SerializeField] private Transform bulletExit;
    private GameObject newBullet;
    private float shootSpeed = 300f;
    private bool canShoot = true;

    //Bool de Reparation
    private bool canRepare = true;


    [Header("Dectection")]
    [SerializeField] private Transform canonTurret;
    public int detectionIndex = 10;


    [Header("Raycast")]
    private RaycastHit rayHit;
    Vector3 direction;

    [Header("State Machine")]
    public turretStatut state = turretStatut.None;
    public turretStatut nextState = turretStatut.None;

    [Header("Navigation")]
    private NavMeshAgent navMesh;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private GameObject pointFuite;

    private Vector3 exDestination;
    private bool goA = true;
    private bool goB = false;

    //Bool: le tank a t'il etait detecter ?
    private bool tankdetected = false;

    [Header("HP System")]
    public int HP = 3;
    [SerializeField] private Sprite fullHP;
    [SerializeField] private Sprite twoHP;
    [SerializeField] private Sprite oneHP;
    [SerializeField] private Image hpBar;

    public enum turretStatut
    {
        None,
        Searching,
        Shooting,
        Patrol,
        Alert,
        RunAway,
        Repare,
    }

    private void Start()
    {
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        state = turretStatut.Patrol;
        navMesh.SetDestination(pointA.transform.position);
    }

    // dans l'ordre : on vérifie si transition a été demandée 
    // si oui on applique l'action de transition s'il y en a une
    // dans tous les cas on applique le comportement de l'état en cours
    void Update()
    {
        if (CheckForTransition())
        {
            TransitionOrChangeState();
        }
        StateBehavior();

        HpSystem();
    }

    // on fait un switch par état, chaque état n'accepte que ses propres transitions.
    private bool CheckForTransition()
    {
        switch (state)
        {
            case turretStatut.None:
                break;

            /*case turretStatut.Searching:
                if(tankdetected)
                {
                    nextState = turretStatut.Shooting;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;*/

            case turretStatut.Shooting:

                if (!tankdetected)
                {
                    nextState = turretStatut.Patrol;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;

            case turretStatut.Patrol:

                if (tankdetected && nextState != turretStatut.RunAway)
                {
                    nextState = turretStatut.Alert;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;

            case turretStatut.Alert:

                if (!tankdetected && nextState != turretStatut.RunAway)
                {
                    nextState = turretStatut.Patrol;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;

            case turretStatut.RunAway:
                if (navMesh.remainingDistance < 0.5f)
                {
                    nextState = turretStatut.Repare;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;

            case turretStatut.Repare:
                if (HP == 3)
                {
                    nextState = turretStatut.Patrol;
                    return true;
                }
                if (HP == 1)
                {
                    nextState = turretStatut.RunAway;
                    return true;
                }
                break;
        }
        return false;
    }

    // on applique les actions associées au transitions
    private void TransitionOrChangeState()
    {
        switch (nextState)
        {
            case turretStatut.None:
                break;

            /*case turretStatut.Searching:
                 break;*/

            case turretStatut.Shooting:
                break;

            case turretStatut.Patrol:
                navMesh.SetDestination(exDestination);
                break;

            case turretStatut.Alert:
                exDestination = navMesh.destination;
                break;

            case turretStatut.RunAway:
                break;

            case turretStatut.Repare:
                break;
        }
        state = nextState;
    }

    // on applique le comportement des états
    private void StateBehavior()
    {
        switch (state)
        {
            case turretStatut.None:
                break;

            //////////////////////////

            /*case turretStatut.Searching:
                 Detecte();
                 break;*/

            ///////////////////////////

            case turretStatut.Shooting:
                TurnTurret();
                if (canShoot == true)
                {
                    canShoot = false;
                    StartCoroutine(Fire());
                }
                break;

            ///////////////////////////

            case turretStatut.Patrol:
                Detecte();
                if (goA)
                {
                    print("Going A");
                    if (navMesh.remainingDistance < 0.5f)
                    {
                        navMesh.SetDestination(pointB.transform.position);
                        goA = false;
                        goB = true;
                    }
                }
                else if (goB)
                {
                    print("Going B");
                    if (navMesh.remainingDistance < 0.5f)
                    {
                        navMesh.SetDestination(pointA.transform.position);
                        goA = true;
                        goB = false;
                    }
                }
                break;

            //////////////////////////

            case turretStatut.Alert:
                Detecte();
                if (player != null)
                    navMesh.SetDestination(player.transform.position);

                TurnTurret();
                if (canShoot == true)
                {
                    canShoot = false;
                    StartCoroutine(Fire());
                }
                print("hunt player");
                break;

            ///////////////////////////

            case turretStatut.RunAway:
                navMesh.SetDestination(pointFuite.transform.position);
                break;

            ///////////////////////////

            case turretStatut.Repare:
                if (HP < 3 && canRepare == true)
                {
                    canRepare = false;
                    StartCoroutine(Reparing());
                }
                break;
        }
    }

    IEnumerator Reparing()
    {
        yield return new WaitForSeconds(1);
        HP++;
        canRepare = true;
    }

    private void Detecte()
    {
        if (player.gameObject != null)
        {
            direction = Vector3.Normalize(player.transform.position - canonTurret.position);

            if (Physics.Raycast(canonTurret.position, direction, out rayHit, detectionIndex))
            {
                if (rayHit.collider.gameObject.CompareTag("Player"))
                {
                    tankdetected = true;
                }
                else
                    tankdetected = false;

                Debug.DrawRay(canonTurret.position, direction, Color.green);
            }
        }
    }

    private void TurnTurret()
    {
        if (tankdetected)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z) * Time.deltaTime);
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

    private void HpSystem()
    {
        switch (HP)
        {
            case 3:
                hpBar.sprite = fullHP;
                break;

            case 2:
                hpBar.sprite = twoHP;
                break;

            case 1:
                hpBar.sprite = oneHP;
                break;

            case 0:
                Destroy(this.gameObject);
                player.GetComponent<Player>().nbrOfTarget--;
                break;
        }
    }
}
