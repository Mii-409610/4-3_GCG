using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletLauncher : MonoBehaviour, IWeaponControl
{
    [Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float speed = 0.0f;         // 弾の速さ
    public float deleteTime = 0.0f;     // 弾の寿命
    public int maxAmmo = 5;             // 最大弾数（例：5発）
    private int currentAmmo;            // 現在弾数

    [Header("弾数・リロード設定")]
    public float reloadTime = 2.0f;       // リロード時間（秒）
    private bool isReloading = false;   // リロード中フラグ

    [Header("UI")]
    public Image reloadGauge;

    [Header("効果音")]
    public AudioClip shotSE;
    private AudioSource audioSource;

    private bool isCurrentWeapon = false;

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

    void Update()
    {
        if (!isCurrentWeapon || isReloading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAmmo <= 0) return;
            FireCannon();
        }
    }

    void FireCannon()
    {
        currentAmmo--;
        // カメラの正面方向と位置を取得
        GameObject cam = GameObject.Find("PlayerCamera");
        Vector3 forward = cam.transform.forward;
        Vector3 spawnPos = transform.position + forward * 1.0f + Vector3.up * 1.5f;

        // 弾を生成
        GameObject copy = Instantiate(bullet, spawnPos, Quaternion.LookRotation(forward));
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        rb.velocity = forward * speed;

        // 一定時間後に自動で削除
        Destroy(copy, deleteTime);

        // SEを再生
        if (shotSE != null)
        {
            audioSource.PlayOneShot(shotSE);
        }

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
