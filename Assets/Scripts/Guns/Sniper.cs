using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public Animator MyAnimator;

    [Header("Settings")]
    public bool AtesEdebilirMi;
    private float IceridenAtesEtmeSikligi;
    public float DisaridanAtesEtmeSiklik;
    public float Menzil;
    public GameObject Cross;
    public GameObject Scope;


    [Header("Sounds")]
    public AudioSource FireSound;
    public AudioSource ReloadSound;
    public AudioSource BulletEndSound;
    public AudioSource PickAmmoSound;


    [Header("Effects")]
    public ParticleSystem FireEffect;
    public ParticleSystem MermiIzi;
    public ParticleSystem KanEfekti;


    [Header("Others")]
    public Camera MyCam;
    private float CamFieldPov;
    public float ZoomPov;

    [Header("Gun Settings")] 
    public string Gun_Name;
    int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    int KalanMermi;
    public TextMeshProUGUI ToplamMermi_Text;
    public TextMeshProUGUI KalanMermi_Text;
    public bool KovanCiksinMi;
    public GameObject KovanCikisNoktasi;
    public GameObject KovanObjesi;
    public CreateAmmoPack CreateAmmoPackManagement;
    public float DamagePower;

    void SarjorDoldurmaTeknikFonksiyon(string Tur)
    {
        switch (Tur)
        {
            case "MermiVar":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    int OlusanToplamDeger =  KalanMermi + ToplamMermiSayisi;
                    if (OlusanToplamDeger > SarjorKapasitesi)
                    {
                        KalanMermi = SarjorKapasitesi;
                        ToplamMermiSayisi = OlusanToplamDeger - SarjorKapasitesi;
                        PlayerPrefs.SetInt(Gun_Name + "_Bullet", ToplamMermiSayisi);
                    }
                    else
                    {
                        KalanMermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(Gun_Name + "_Bullet", 0);
                    }
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(Gun_Name + "_Bullet", ToplamMermiSayisi);
                }
                
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "MermiYok":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    KalanMermi = ToplamMermiSayisi;
                    ToplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(Gun_Name + "_Bullet", 0);
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(Gun_Name + "_Bullet", ToplamMermiSayisi);
                }
                
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "NormalYaz":
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;
        }
    }

    void Baslangic_Mermi_Doldur()
    {
        if (ToplamMermiSayisi <= SarjorKapasitesi)
        {
           
                KalanMermi = ToplamMermiSayisi;
                ToplamMermiSayisi = 0;
                PlayerPrefs.SetInt(Gun_Name + "_Bullet", 0);  
        }
        else
        {
            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi -= SarjorKapasitesi;
            PlayerPrefs.SetInt(Gun_Name + "_Bullet", ToplamMermiSayisi);
        }
    }

    void Start()
    {
        ToplamMermiSayisi = PlayerPrefs.GetInt(Gun_Name + "_Bullet");
        Baslangic_Mermi_Doldur();
        SarjorDoldurmaTeknikFonksiyon("NormalYaz");
        MyAnimator = GetComponent<Animator>();
        KalanMermi = SarjorKapasitesi;
        CamFieldPov = MyCam.fieldOfView;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (AtesEdebilirMi && Time.time > IceridenAtesEtmeSikligi && KalanMermi != 0)
            {
                if (!GameControl.OyunDurduMu)
                {
                    AtesEt();
                    IceridenAtesEtmeSikligi = Time.time + DisaridanAtesEtmeSiklik;
                }
                   
            }
            if(KalanMermi == 0)
            {
                BulletEndSound.Play();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
            {
                ReloadSound.Play();
                MyAnimator.Play("Reload");
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeAmmo();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Scope(true);
            
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Scope(false);
            
        }

        void AtesEt()
        {
            if (KovanCiksinMi)
            {
                GameObject Obje = Instantiate(KovanObjesi, KovanCikisNoktasi.transform.position, KovanCikisNoktasi.transform.rotation);
                Rigidbody Rb1 = Obje.GetComponent<Rigidbody>();
                Rb1.AddRelativeForce(new Vector3(-10f,1,0) * 45);
            }

            MyAnimator.Play("Fire");
            FireSound.Play();
            FireEffect.Play();

            KalanMermi--;
            KalanMermi_Text.text = KalanMermi.ToString();

            RaycastHit Hit;
            
            if (Physics.Raycast(MyCam.transform.position, MyCam.transform.forward, out Hit, Menzil))
            {
                if (Hit.transform.gameObject.CompareTag("Dusman"))
                {
                    Instantiate(KanEfekti, Hit.point, Quaternion.LookRotation(Hit.normal));

                    Hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(DamagePower); //her ateþ ettiðimde düþman verdiðimiz deðer kadar hasar alacak
                }
                else if (Hit.transform.gameObject.CompareTag("DevrilebilirObje"))
                {
                    Rigidbody Rb = Hit.transform.gameObject.GetComponent<Rigidbody>();
                    Rb.AddForce(-Hit.normal * 50000000000f);
                }
                else
                {
                    Instantiate(MermiIzi, Hit.point, Quaternion.LookRotation(Hit.normal));
                }
            }
            
        }

        void Scope(bool durum)
        {
            if (durum)
            {
                Cross.SetActive(false);
                MyCam.cullingMask = ~ (1 << 6);
                MyAnimator.SetBool("Zoom", durum);
                MyCam.fieldOfView = ZoomPov;
                this.Scope.SetActive(true);
            }
            else
            {
                Cross.SetActive(true);
                MyCam.cullingMask = -1;
                MyAnimator.SetBool("Zoom", durum);
                MyCam.fieldOfView = CamFieldPov;
                this.Scope.SetActive(false);
            }
            
        }

        void TakeAmmo()
        {
            RaycastHit Hit;

            if (Physics.Raycast(MyCam.transform.position, MyCam.transform.forward, out Hit, 3))
            {
                if (Hit.transform.gameObject.CompareTag("Ammo"))
                {
                    MermiKaydet(Hit.transform.gameObject.GetComponent<PickupAmmo>().OlusanSilahinTuru, 
                        Hit.transform.gameObject.GetComponent<PickupAmmo>().OlusanMermiSayisi);
                    CreateAmmoPackManagement.RemovePoints(Hit.transform.gameObject.GetComponent<PickupAmmo>().Point);
                    Destroy(Hit.transform.parent.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<PickupAmmo>().OlusanSilahinTuru,
                other.transform.gameObject.GetComponent<PickupAmmo>().OlusanMermiSayisi);
            CreateAmmoPackManagement.RemovePoints(other.transform.gameObject.GetComponent<PickupAmmo>().Point);
            Destroy(other.transform.parent.gameObject);

        }
        if (other.gameObject.CompareTag("HealthBox"))
        {
            CreateAmmoPackManagement.GetComponent<GameControl>().TakeHealth();
            Destroy(other.transform.gameObject);
            CreateHealthBox.IsHealthBox = false;
        }
        if (other.gameObject.CompareTag("BombBox"))
        {
            CreateAmmoPackManagement.GetComponent<GameControl>().TakeBomb();
            CreateBombBox.IsBombBox = false;
            Destroy(other.transform.gameObject);
        }
    }

    void Reload()
    {
        if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
            {
                if (KalanMermi != 0)
                {
                    SarjorDoldurmaTeknikFonksiyon("MermiVar");
                }
                else
                {
                    SarjorDoldurmaTeknikFonksiyon("MermiYok");
                }
                /*if (!ReloadSound.isPlaying)
                {
                    ReloadSound.Play();
                }*/
            }
    }

    void MermiKaydet(string SilahTuru, int MermiSayisi)
    {
        PickAmmoSound.Play();

        switch (SilahTuru)
        {
            case "Rifle":
                PlayerPrefs.SetInt("Rifle_Bullet", PlayerPrefs.GetInt("Rifle_Bullet") + MermiSayisi);
                break;

            case "Shotgun":
                PlayerPrefs.SetInt("Shotgun_Bullet", PlayerPrefs.GetInt("Shotgun_Bullet") + MermiSayisi);
                break;

            case "Sniper":
                ToplamMermiSayisi += MermiSayisi;
                PlayerPrefs.SetInt(Gun_Name + "_Bullet", ToplamMermiSayisi);
                SarjorDoldurmaTeknikFonksiyon("NormalYaz");
                break;

            case "Magnum":
                PlayerPrefs.SetInt("Magnum_Bullet", PlayerPrefs.GetInt("Magnum_Bullet") + MermiSayisi);
                break;
        }
    }

    
    
}
