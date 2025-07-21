using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletShotgun : MonoBehaviour, IWeaponControl
{
    [Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float speed = 0.0f;         // 弾の速さ
    public float deletetime = 0.0f;     // 弾の寿命
    public int maxAmmo = 5;             // 最大弾数（例：5発）
    private int currentAmmo;            // 現在弾数
    public float verticalAngle = 0.0f;    // 未使用（上下角）
    public float horizontalAngle = 0.0f;  // 未使用（左右角）

    [Header("弾数・リロード設定")]
    public float reloadTime = 2.0f;       // リロード時間（秒）
    private bool isReloading = false;   // リロード中フラグ

    [Header("UI")]
    public Image reloadGauge;

    [Header("効果音")]
    public AudioClip shotSE;
    private AudioSource audioSource;

    private bool isCurrentWeapon = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1f;
    }

    public void SetWeaponActive(bool isActive)
    {
        isCurrentWeapon = isActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCurrentWeapon || isReloading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAmmo <= 0) return;
            FireShotgun();
        }
    }

    void FireShotgun()
    {
        currentAmmo--;
        Vector3 basePos = transform.position + Camera.main.transform.forward * 4f + Vector3.up * 1.5f;
        Vector3 baseDir = Camera.main.transform.forward;

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3 spreadDir = baseDir + Camera.main.transform.up * y * 0.3f + Camera.main.transform.right * x * 0.1f;
                spreadDir.Normalize();
                GameObject b = Instantiate(bullet, basePos, Quaternion.LookRotation(spreadDir));
                b.GetComponent<Rigidbody>().velocity = spreadDir * speed;
                Destroy(b, deletetime);
            }
        }

        if (shotSE != null)
            audioSource.PlayOneShot(shotSE);

        if (currentAmmo <= 0 && !isReloading)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        float timer = 0f;
        reloadGauge.fillAmount = 0f;

        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            reloadGauge.fillAmount = Mathf.Clamp01(timer / reloadTime);
            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        reloadGauge.fillAmount = 1f;
    }
}
