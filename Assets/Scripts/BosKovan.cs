using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosKovan : MonoBehaviour
{
    public AudioSource KovanSesi;

    void Start()
    {
        KovanSesi = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            KovanSesi.Play();

            if (!KovanSesi.isPlaying)
            {
                Destroy(gameObject,1f);
            }
        }
    }

    void Update()
    {
        
    }
}
