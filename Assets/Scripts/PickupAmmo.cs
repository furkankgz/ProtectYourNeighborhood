using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupAmmo : MonoBehaviour
{
    string[] Guns =
    {
        "Magnum",
        "Shotgun",
        "Sniper",
        "Rifle"
    };

    int[] BulletCount =
    {
        10,
        15,
        20,
        30
    };

    public string OlusanSilahinTuru;
    public int OlusanMermiSayisi;
    public List<Sprite> GunImages = new List<Sprite>();
    public Image GunImage;
    public int Point;


    void Start()
    {
        int GelenAnahtar = Random.Range(0, Guns.Length);
        OlusanSilahinTuru = Guns[GelenAnahtar];
        OlusanMermiSayisi = BulletCount[Random.Range(0, BulletCount.Length - 1)];

        GunImage.sprite = GunImages[GelenAnahtar];

        /*OlusanSilahinTuru = "Rifle";
        OlusanMermiSayisi = 10;*/

        Debug.Log(OlusanSilahinTuru);
        Debug.Log(OlusanMermiSayisi);
    }
}
