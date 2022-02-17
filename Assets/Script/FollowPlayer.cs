using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject tankCanon;

    // Update is called once per frame
    void Update()
    {
        transform.position = tankCanon.transform.position;
        transform.rotation = tankCanon.transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
