using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHealthBox : MonoBehaviour
{
    public List<GameObject> HealthBoxPoint = new List<GameObject>();
    public GameObject HealthBox;
    public static bool IsHealthBox;
    public float CreateHealthBoxTime;
    private int RandomNumber;

    void Start()
    {
        IsHealthBox = false;
        StartCoroutine(CreateHealthBoxEnumerator());
    }

    IEnumerator CreateHealthBoxEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(CreateHealthBoxTime);
            if (!IsHealthBox)
            {
                RandomNumber = UnityEngine.Random.Range(0, 5);
                Instantiate(HealthBox, HealthBoxPoint[RandomNumber].transform.position, HealthBoxPoint[RandomNumber].transform.rotation);
                IsHealthBox = true;
            }
            
        }
    }
}
