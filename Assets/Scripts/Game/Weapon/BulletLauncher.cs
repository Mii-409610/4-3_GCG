using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランチャーの弾を制御するクラス
/// </summary>
public class BulletLauncher : MonoBehaviour
{
    // ======= Inspectorで設定する項目 =======
    [SerializeField, Header("銃口")]
    public Transform shootPoint;

    [SerializeField, Header("カメラ")]
    public Camera playerCamera;

    [SerializeField, Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float shootForce = 0.0f;     // 弾の発射速度
    public float deleteTime = 0.0f;     // 弾の寿命(秒)
    public int maxCapacity = 5;         // 最大弾数
    public float fireInterval = 1.5f;   // 発射間隔

    [SerializeField, Header("リロード設定")]
    public float reloadTime = 2.0f;     // リロード所要時間(秒)

    [SerializeField, Header("反動設定")]
    public Transform recoilTarget;              // 銃のモデル
    public float recoilRotationStrength = 5.0f; // 反動の強さ(回転)
    public float recoilReturnSpeed = 8.0f;      // 反動が戻る速さ

    [SerializeField, Header("UI")]
    public Image reloadGauge;           // リロードゲージのUI

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

        // リロードゲージを満タンに初期化
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1.0f;
    }

    void Update()
    {
        if (IsGameManager.isGameEnded) return;
        if (PouseManager.isPaused) return;

        // === 反動の戻し処理 ===
        if (recoilTarget != null)
        {
            currentRecoilEuler = Vector3.Lerp(currentRecoilEuler, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            Quaternion baseRotation = Quaternion.Euler(0f, -90f, 0f);
            recoilTarget.localRotation = baseRotation * Quaternion.Euler(currentRecoilEuler);
        }

        // === 発射処理（長押し対応＋クールタイム制御） ===
        // 発射処理（長押し対応＋クールタイム制御＋弾数チェック）
        if (Input.GetKey(KeyCode.Space)
            && currentBullet > 0                            // 弾が残っている
            && !WeaponReloadManager.IsReloading(weaponName) // リロード中でない
            && Time.time - lastShotTime >= fireInterval)    // クールタイム経過
        {
            FireLauncher();
            lastShotTime = Time.time;
        }

        // ===== 弾切れ時リロード開始 =====
        if (currentBullet <= 0 && !WeaponReloadManager.IsReloading(weaponName))
        {
            WeaponReloadManager.StartReload(
                weaponName,
                reloadTime,
                () => { currentBullet = maxCapacity; },
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

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    void FireLauncher()
    {
        currentBullet--; // 弾を１発消費

        // カメラ中央からレイを飛ばし、ターゲット座標を取得
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Vector3 targetPoint;

        // Raycastのレイヤー設定(ignoreLayerが指定されていれば除外)
        int layerMask = Physics.DefaultRaycastLayers;
        if (ignoreLayer.value != 0) layerMask = ~ignoreLayer.value;

        // Raycastが何かに当たった場合はその地点、当たらなければ遠方
        if (Physics.Raycast(ray, out hit, 10000f, layerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        // 銃口からターゲットへの方向ベクトルを計算
        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        // 弾を生成（位置は銃口）
        GameObject copy = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = direction * shootForce; // 弾に速度を与える

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
        Destroy(copy, deleteTime);

        // SE再生
        AudioManager.Instance.PlaySE(SEID.SE_Launcher);
    }
}
