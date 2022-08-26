using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mermi : MonoBehaviour
{
    Rigidbody Rb;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.velocity = transform.forward * 250;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
