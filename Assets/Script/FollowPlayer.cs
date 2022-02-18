using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject tankCanon;

    void Update()
    {
        transform.position = tankCanon.transform.position;
    }
}
