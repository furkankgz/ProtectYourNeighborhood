using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float Power;
    public float Distance;
    public float YukariGüc;
    public ParticleSystem ExplosionEffect;
    AudioSource ExplosionSound;

    void Start()
    {
        ExplosionSound = GetComponent<AudioSource>();
        
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Destroy(gameObject, 5f);
            Patlama();
        }
    }

    void Patlama()
    {
        Vector3 PatlamaPozisyonu = transform.position;
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(PatlamaPozisyonu, Distance);
        ExplosionSound.Play();
        foreach (var hit in colliders)
        {
            Rigidbody Rb = hit.GetComponent<Rigidbody>();
            if (hit != null && Rb)
            {
                if (hit.gameObject.CompareTag("Dusman"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().Dead();
                }

                Rb.AddExplosionForce(Power, PatlamaPozisyonu, Distance, 1, ForceMode.Impulse);
            }
        }
    }
}
