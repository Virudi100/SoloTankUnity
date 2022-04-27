using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _tankCanon;

    void Update()
    {
        if(_tankCanon != null) //si le joueur est vivant
        {
            transform.position = _tankCanon.transform.position;
        }  
    }
}
