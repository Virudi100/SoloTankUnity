using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject firepoint;
    RaycastHit hit;
    Vector3 vP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(gameObject.transform.position,gameObject.transform.forward, out hit))
        {
            vP = hit.point;
            lr.SetPosition(0, new Vector3( 0,0,vP.z));
        }
    }
}
