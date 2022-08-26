using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAmmoPack : MonoBehaviour
{
    public List<GameObject> AmmoPackPoint = new List<GameObject>();
    private List<int> Points = new List<int>();
    public GameObject AmmoPack;
    public static bool IsAmmoPack;
    public float CreateAmmoPackTime;
    private int RandomNumber;

    void Start()
    {
        IsAmmoPack = false;
        StartCoroutine(CreateAmmoPackEnumerator());
    }

    IEnumerator CreateAmmoPackEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(CreateAmmoPackTime);
            RandomNumber = UnityEngine.Random.Range(0, 5);

            if (!Points.Contains(RandomNumber))
            {
                Points.Add(RandomNumber);
            }
            else
            {
                RandomNumber = UnityEngine.Random.Range(0, 5);
                continue;
            }
            GameObject Object = Instantiate(AmmoPack, AmmoPackPoint[RandomNumber].transform.position, AmmoPackPoint[RandomNumber].transform.rotation);
            Object.gameObject.GetComponentInChildren<PickupAmmo>().Point = RandomNumber;
        }
    }

    public void RemovePoints(int deger)
    {
        Points.Remove(deger);
    }
}
