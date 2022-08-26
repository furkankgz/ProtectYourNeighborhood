using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBombBox : MonoBehaviour
{
    public List<GameObject> BombBoxPoint = new List<GameObject>();
    public GameObject BombBox;
    public static bool IsBombBox;
    public float CreateBombBoxTime;
    private int RandomNumber;

    void Start()
    {
        IsBombBox = false;
        StartCoroutine(CreateBombBoxEnumerator());
    }

    IEnumerator CreateBombBoxEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(CreateBombBoxTime);
            if (!IsBombBox)
            {
                RandomNumber = UnityEngine.Random.Range(0, 5);
                Instantiate(BombBox, BombBoxPoint[RandomNumber].transform.position, BombBoxPoint[RandomNumber].transform.rotation);
                IsBombBox = true;
            }
            
        }
    }
}
