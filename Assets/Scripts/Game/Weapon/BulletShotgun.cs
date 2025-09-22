using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ショットガンの弾を制御するクラス
/// </summary>
public class BulletShotgun : MonoBehaviour
{
    // ======= Inspectorで設定する項目 =======
    [SerializeField, Header("銃口")]
    public Transform shootPoint;

    [SerializeField, Header("カメラ")]
    public Camera playerCamera;

    [SerializeField, Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float shootForce = 20.0f;    // 弾の発射速度
    public float deleteTime = 2.0f;     // 弾の寿命(秒)
    public int maxCapacity = 5;         // 最大弾数
    public float fireInterval = 0.5f;   // 発射間隔

    [SerializeField, Header("散弾の個数")]
    public int pelletCount = 9;

    [SerializeField, Header("拡散角度")]
    public float spreadAngle = 8.0f;

    [SerializeField, Header("リロード時間")]
    public float reloadTime = 2.0f;     // リロード所要時間(秒)

    [SerializeField, Header("反動設定")]
    public Transform recoilTarget;              // 銃のモデル
    public float recoilRotationStrength = 5.0f; // 反動の強さ(回転)
    public float recoilReturnSpeed = 8.0f;      // 反動が戻る速さ

    [SerializeField, Header("UI")]
    public Image reloadGauge;           // リロードゲージのUI

    [SerializeField, Header("効果音")]
    public AudioClip shotSE;            // 発射音
    private AudioSource audioSource;    // 効果音再生用

    [SerializeField, Header("無視するレイヤー")]
    public LayerMask ignoreLayer;       // Raycast時に無視するレイヤー

    // ======= 内部状態 =======
    private int currentBullet;                          // 現在の弾数
    private string weaponName;                          // 武器名(リロード管理用)
    private Vector3 currentRecoilEuler = Vector3.zero;  // 現在の反動角度
    private float lastShotTime = -999f;                 // クールタイム管理用

    void Start()
    {
        // 武器名を設定
        weaponName = gameObject.name;

        // 弾数を最大にセット
        currentBullet = maxCapacity;

        // AudioSourceを取得または追加
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // ゲージを満タンに
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1.0f;
    }

    void Update()
    {
        if (IsGameManager.isGameEnded) return;

        // === 反動の戻し処理 ===
        if (recoilTarget != null)
        {
            currentRecoilEuler = Vector3.Lerp(currentRecoilEuler, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            Quaternion baseRotation = Quaternion.Euler(0f, -90f, 0f);
            recoilTarget.localRotation = baseRotation * Quaternion.Euler(currentRecoilEuler);
        }

        // === 発射処理 ===
        if (Input.GetKey(KeyCode.Space) && currentBullet > 0)
        {
            // クールタイムチェック
            if (Time.time - lastShotTime >= fireInterval)
            {
                FireShotgun();
                lastShotTime = Time.time;
            }
        }

        // ===== 弾切れ時リロード開始 =====
        if (currentBullet <= 0 && !WeaponReloadManager.IsReloading(weaponName))
        {
            WeaponReloadManager.StartReload(
                weaponName,
                reloadTime,
                () => { currentBullet = maxCapacity; }, // リロード完了時に弾数を最大に
                reloadGauge
            );
        }

        // ===== リロードUI更新 =====
        if (WeaponReloadManager.IsReloading(weaponName))
        {
            if (reloadGauge != null)
                reloadGauge.fillAmount = WeaponReloadManager.GetReloadProgress(weaponName);
            return;
        }
    }

    void FireShotgun()
    {
        currentBullet--; // 弾を１発消費

        // カメラ中央からレイを飛ばし、基準となる発射方向を取得
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 baseDir = ray.direction.normalized;  // カメラ正面方向
        Vector3 basePos = shootPoint.position;       // 発射位置

        for (int i = 0; i < pelletCount; i++)
        {
            // 拡散角度内でランダムな方向を作成
            float ySpread = Random.Range(-spreadAngle, spreadAngle);
            float xSpread = Random.Range(-spreadAngle, spreadAngle);
            Vector3 spreadDir = Quaternion.Euler(xSpread, ySpread, 0) * baseDir;

            // 弾生成＆発射
            GameObject b = Instantiate(bullet, basePos, Quaternion.LookRotation(spreadDir));
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = spreadDir * shootForce;

            // 反動付与
            if (recoilTarget != null)
            {
                Vector3 kick = new Vector3(
                    Random.Range(-recoilRotationStrength, -recoilRotationStrength / 2),
                    Random.Range(-recoilRotationStrength * 0.5f, recoilRotationStrength * 0.5f),
                    Random.Range(-recoilRotationStrength * 0.5f, recoilRotationStrength * 0.5f)
                );
                currentRecoilEuler += kick;
            }

            // 弾の消滅
            Destroy(b, deleteTime);
        }

        // 発射音
        if (shotSE != null)
            audioSource.PlayOneShot(shotSE);
    }
}