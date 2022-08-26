using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityStandardAssets.Characters.FirstPerson;


public class GameControl : MonoBehaviour
{
    private int GunNumber;

    [Header("Health Settings")]
    private float Health = 100;
    public Image HealthBar;

    [Header("Gun Settings")]
    public GameObject[] Guns;
    public AudioSource ChangeSound;
    public GameObject Bomb;
    public GameObject BombPoint; //Bombanýn çýkýþ noktasý
    public Camera MyCam; // Bombanýn gideceði yön

    [Header("Enemy Settings")] 
    public GameObject[] Enemies;
    public GameObject[] ExitPoints;
    public GameObject[] TargetPoints;
    public float EnemySpawnTime;
    public TextMeshProUGUI KalanDusmanTextMesh;
    public int BaslangictaDusmanSayisi;
    public static int KalanDusmanSayisi;

    [Header("Other Settings")] 
    public GameObject GameOverCanvas;
    public GameObject YouWinCanvas;
    public GameObject PauseCanvas;
    public AudioSource OyunIciSes;
    public TextMeshProUGUI HealthBoxTextMesh;
    public TextMeshProUGUI BombTextMesh;
    public AudioSource NoItem; // can ya da bomba olmadýðý zaman kullanmaya çalýþýrsak bu sesi çal
    public static bool OyunDurduMu;

    void Start()
    {
        BaslangicIslemleri();
    }

    void BaslangicIslemleri()
    {
        OyunDurduMu = false;
        if (!PlayerPrefs.HasKey("IsGameStarted"))
        {
            PlayerPrefs.SetInt("Rifle_Bullet", 90);
            PlayerPrefs.SetInt("Shotgun_Bullet", 21);
            PlayerPrefs.SetInt("Magnum_Bullet", 21);
            PlayerPrefs.SetInt("Sniper_Bullet", 15);
            PlayerPrefs.SetInt("HealthBox_Count", 15);
            PlayerPrefs.SetInt("Bomb_Count", 1);
            PlayerPrefs.SetInt("IsGameStarted", 5);
        }

        OyunIciSes = GetComponent<AudioSource>();
        KalanDusmanSayisi = BaslangictaDusmanSayisi;

        GunNumber = 0;

        KalanDusmanTextMesh.text = BaslangictaDusmanSayisi.ToString();
        HealthBoxTextMesh.text = PlayerPrefs.GetInt("HealthBox_Count").ToString();
        BombTextMesh.text = PlayerPrefs.GetInt("Bomb_Count").ToString();

        StartCoroutine(SpawnEnemy());

    }

    IEnumerator SpawnEnemy() // Düþmanlarý farklý noktalardan spawn etme
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemySpawnTime);

            if (BaslangictaDusmanSayisi != 0)
            {
                int Enemy = Random.Range(0, 5);
                int ExitPoint = Random.Range(0, 2);
                int TargetPoint = Random.Range(0, 2);
                GameObject Object = Instantiate(Enemies[Enemy], ExitPoints[ExitPoint].transform.position, Quaternion.identity);
                Object.GetComponent<Enemy>().SetATarget(TargetPoints[TargetPoint]);
                BaslangictaDusmanSayisi--;
            }

            
        }
    }

    void Update()
    {
        Buttons();
    }

    void Buttons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !OyunDurduMu)
        {
            ChangeGun(0);
        } // 1'e basýnca Dizideki 1. sýradaki silah gelsin

        if (Input.GetKeyDown(KeyCode.Alpha2) && !OyunDurduMu)
        {
            ChangeGun(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !OyunDurduMu)
        {
            ChangeGun(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && !OyunDurduMu)
        {
            ChangeGun(3);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !OyunDurduMu)
        {
            ChangeGunWithQ();
        }

        if (Input.GetKeyDown(KeyCode.G) && !OyunDurduMu)
        {
            BombaAt();
        }

        if (Input.GetKeyDown(KeyCode.E) && !OyunDurduMu)
        {
            FillHealth();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !OyunDurduMu)
        {
            Pause();
        }
    }

    public void TakeDamage(float DamagePower) // Düþmanýn verdiði hasarý healthbar'da yansýtan fonksiyon
    {
        Health -= DamagePower;
        HealthBar.fillAmount = Health / 100;
        if (Health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurduMu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
    }

    void ChangeGun(int Number)
    {
        ChangeSound.Play();
        foreach (var gun in Guns)
        {
            gun.SetActive(false);
        }

        GunNumber = Number;
        Guns[Number].SetActive(true);
    } // silah deðiþtirme

    void ChangeGunWithQ()
    {
        ChangeSound.Play();
        foreach (var gun in Guns)
        {
            gun.SetActive(false);
        }

        if (GunNumber == 3)
        {
            GunNumber = 0;
            Guns[GunNumber].SetActive(true);
            
        }
        else
        {
            GunNumber++;
            Guns[GunNumber].SetActive(true);
        }
    } // Q tuþu ile silah deðiþtirme

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        OyunDurduMu = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
    }

    public void FillHealth()
    {
        if (PlayerPrefs.GetInt("HealthBox_Count") != 0 && Health != 100)
        {
            Health = 100;
            HealthBar.fillAmount = Health / 100;
            PlayerPrefs.SetInt("HealthBox_Count", PlayerPrefs.GetInt("HealthBox_Count") - 1);
            HealthBoxTextMesh.text = PlayerPrefs.GetInt("HealthBox_Count").ToString();
        }
        else
        {
            NoItem.Play();
        }
        
    }

    public void TakeHealth()
    {
        PlayerPrefs.SetInt("HealthBox_Count", PlayerPrefs.GetInt("HealthBox_Count") + 1);
        HealthBoxTextMesh.text = PlayerPrefs.GetInt("HealthBox_Count").ToString();
    }

    public void TakeBomb()
    {
        PlayerPrefs.SetInt("Bomb_Count", PlayerPrefs.GetInt("Bomb_Count") + 1);
        BombTextMesh.text = PlayerPrefs.GetInt("Bomb_Count").ToString();
    }

    void BombaAt()
    {
        if (PlayerPrefs.GetInt("Bomb_Count") != 0)
        {
            GameObject Object = Instantiate(Bomb, BombPoint.transform.position, BombPoint.transform.rotation);
            Rigidbody Rb = Object.GetComponent<Rigidbody>();
            Vector3 BombaAcýsý = Quaternion.AngleAxis(90, MyCam.transform.forward) * MyCam.transform.forward;
            Rb.AddForce(BombaAcýsý * 250f);

            PlayerPrefs.SetInt("Bomb_Count", PlayerPrefs.GetInt("Bomb_Count") - 1);
            BombTextMesh.text = PlayerPrefs.GetInt("Bomb_Count").ToString();
        }
        else
        {
            NoItem.Play();
        }
    }

    public void DusmanSayisiGuncelle()
    {
        KalanDusmanSayisi--;
        if (KalanDusmanSayisi <= 0)
        {
            YouWinCanvas.SetActive(true);
            KalanDusmanTextMesh.text = "0";
            Time.timeScale = 0;
        }
        else
        {
            KalanDusmanTextMesh.text = KalanDusmanSayisi.ToString();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurduMu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
    }

    public void Resume()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        OyunDurduMu = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
    }
}
