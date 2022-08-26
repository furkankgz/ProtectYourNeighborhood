using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent Agent;
    private GameObject Target;
    public float EnemyHealth;
    private GameObject GameControl;
    public float EnemyDamagePower; // Düþmanlarýn vermiþ olacaðý birbirinden farklý hasar güçlerini verdiðimiz kýsým
    private Animator MyAnimator;
    
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        GameControl = GameObject.FindWithTag("GameControl");
        Agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        Agent.SetDestination(Target.transform.position);
    }

    public void SetATarget(GameObject Object)
    {
        Target = Object;
    }

    public void TakeDamage(float DamagePower)
    {
        EnemyHealth -= DamagePower;
        if (EnemyHealth <= 0)
        {
            Dead();
            gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyTarget"))
        {
            GameControl.GetComponent<GameControl>().TakeDamage(EnemyDamagePower);
            Dead();
        }
    }

    public void Dead()
    {
        GameControl.GetComponent<GameControl>().DusmanSayisiGuncelle();
        MyAnimator.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }
}
