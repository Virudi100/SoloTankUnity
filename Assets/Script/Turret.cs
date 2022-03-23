using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : MonoBehaviour
{

    [SerializeField] private GameObject player;


    [SerializeField] private GameObject obus;
    [SerializeField] private Transform bulletExit;
    private GameObject newBullet;
    private float shootSpeed = 300f;
    private bool canShoot = true;
    [SerializeField] private Transform canonTurret;
    public int detectionIndex = 10;

    private RaycastHit rayHit;
    Vector3 direction;

    [Header("State Machine")]
    public turretStatut state = turretStatut.None;
    public turretStatut nextState = turretStatut.None;
    private NavMeshAgent navMesh;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;

    private Vector3 exDestination;
    private bool goA = true;
    private bool goB = false;

    private bool tankdetected = false;

    int i = 0;

    public enum turretStatut
    {
        None,
        Searching,
        Shooting,
        Patrol,
        Alert,
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
        if(CheckForTransition())
        {
            TransitionOrChangeState();
        }
        StateBehavior();
    }

    // on fait un switch par état, chaque état n'accepte que ses propres transitions.
    private bool CheckForTransition()
    {
        switch(state)
        {
            case turretStatut.None:
                break;

            case turretStatut.Searching:
                if(tankdetected)
                {
                    nextState = turretStatut.Shooting;
                    return true;
                }
                break;

            case turretStatut.Shooting:

                if(!tankdetected)
                {
                    nextState = turretStatut.Patrol;
                    return true;
                }
                break;

            case turretStatut.Patrol:
               
                if(tankdetected)
                {
                    nextState = turretStatut.Alert;
                    return true;

                }
                break;

            case turretStatut.Alert:
                
                if(!tankdetected)
                {
                    nextState = turretStatut.Patrol;
                    return true;

                }
                break;
        }
        return false;
    }

    // on applique les actions associées au transitions
    private void TransitionOrChangeState()
    {
        switch(nextState)
        {
            case turretStatut.None:
                break;

            case turretStatut.Searching:
                break;

            case turretStatut.Shooting:
                break;

            case turretStatut.Patrol:
                navMesh.SetDestination(exDestination);

                break;

            case turretStatut.Alert:
                exDestination = navMesh.destination;

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

            case turretStatut.Searching:
                Detecte();
                break;

            case turretStatut.Shooting:
                TurnTurret();
                if (canShoot == true)
                {
                    canShoot = false;      
                    StartCoroutine(Fire());
                }
                break;

            case turretStatut.Patrol:
                Detecte();

                if(goA)
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

            case turretStatut.Alert:
                Detecte();
                navMesh.SetDestination(player.transform.position);
                print("hunt player");
                break;
        }
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


    /*private void IsTankDetected()
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
                            state = turretStatut.Shooting;
                        }
                    }
                }
                Debug.DrawRay(canonTurret.position, direction, Color.green);
            }
        }
    }*/


}
