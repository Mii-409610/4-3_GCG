using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ミニガンの弾を制御するクラス
/// </summary>
public class BulletMiniGun : MonoBehaviour
{
    // ======= Inspectorで設定する項目 =======
    [SerializeField, Header("銃口")]
    public Transform shootPoint;

    [SerializeField, Header("カメラ")]
    public Camera playerCamera;

    [SerializeField, Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float shootForce = 100.0f;   // 弾の発射速度
    public float deleteTime = 3.0f;     // 弾の寿命(秒)
    public int maxCapacity = 40;        // 最大弾数

    [SerializeField, Header("リロード時間")]
    public float reloadTime = 2.0f;     // リロード所要時間(秒)

    [SerializeField, Header("発射速度設定")]
    public float maxInterval = 0.5f;   // 撃ち始めの遅い間隔
    public float minInterval = 0.1f;   // 最高速度の間隔
    public float spinUpTime = 2.0f;    // 加速にかかる時間

    [SerializeField, Header("反動設定")]
    public Transform recoilTarget;              // 銃のモデル
    public float recoilRotationStrength = 1.5f; // 反動の強さ(回転)
    public float recoilReturnSpeed = 10.0f;     // 反動が戻る速さ

    [SerializeField, Header("後隙設定")]
    public float cooldownAfterRelease = 0.5f; // 離した後のクールタイム
    private float nextFireReadyTime = 0f;     // 次に撃てる時間

    [SerializeField, Header("UI")]
    public Image reloadGauge;           // リロードゲージのUI

    [SerializeField, Header("効果音")]
    public AudioClip shotSE;
    private AudioSource audioSource;

    [SerializeField, Header("無視するレイヤー")]
    public LayerMask ignoreLayer;       // Raycast時に無視するレイヤー

    // ======= 内部状態 =======
    private int currentBullet;                          // 現在の弾数
    private string weaponName;                          // 武器名(リロード管理用)
    private bool isShooting = false;                    // 発射中かどうか
    private float shootStartTime;                       // 発射を開始した時間
    private Vector3 currentRecoilEuler = Vector3.zero;  // 現在の反動角度

    // Start is called before the first frame update
    void Start()
    {
        // 武器名を設定
        weaponName = gameObject.name;

        // 弾数を最大にセット
        currentBullet = maxCapacity;

        // AudioSourceを取得または追加
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // リロードゲージを満タンに初期化
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameManager.isGameEnded) return;

        // ===== 銃の反動を戻す処理 =====
        if (recoilTarget != null)
        {
            currentRecoilEuler = Vector3.Lerp(currentRecoilEuler, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            Quaternion baseRotation = Quaternion.Euler(0f, -90f, 0f);
            recoilTarget.localRotation = baseRotation * Quaternion.Euler(currentRecoilEuler);
        }

        // ===== 発射入力検出 =====
        bool shootInput = Input.GetKey(KeyCode.Space);

        if (shootInput && !isShooting && currentBullet > 0 && !WeaponReloadManager.IsReloading(weaponName) && Time.time >= nextFireReadyTime)
        {
            isShooting = true;
            shootStartTime = Time.time;
            StartCoroutine(ShootLoop());
        }
        else if (!shootInput && isShooting)
        {
            // 離した瞬間に後隙を発生
            isShooting = false;
            nextFireReadyTime = Time.time + cooldownAfterRelease;
        }

        // ===== 弾切れ時リロード開始 =====
        if (currentBullet <= 0 && !WeaponReloadManager.IsReloading(weaponName))
        {
            WeaponReloadManager.StartReload(
                weaponName, // 武器名
                reloadTime, // リロード時間
                () => { currentBullet = maxCapacity; }, // リロード完了時に弾数を最大に
                reloadGauge // リロードゲージUI
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
    /// 発射処理のコルーチン
    /// </summary>
    private IEnumerator ShootLoop()
    {
        while (isShooting && currentBullet > 0 && !WeaponReloadManager.IsReloading(weaponName))
        {
            FireMiniGun();
            currentBullet--;

            if (currentBullet <= 0)
            {
                WeaponReloadManager.StartReload(
                    weaponName,
                    reloadTime,
                    () => { currentBullet = maxCapacity; },
                    reloadGauge
                );
                yield break;
            }

            // 経過時間から現在の連射速度を計算（スピンアップ処理）
            float t = Mathf.Clamp01((Time.time - shootStartTime) / spinUpTime);
            float currentInterval = Mathf.Lerp(maxInterval, minInterval, t);

            yield return new WaitForSeconds(currentInterval);
        }
    }

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    void FireMiniGun()
    {
        // カメラ中央からレイを飛ばす
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, ~ignoreLayer))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000);

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        GameObject copy = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = direction * shootForce;
        Destroy(copy, deleteTime);

        // 反動付与
        if (recoilTarget != null)
        {
            Vector3 randomKick = new Vector3(
                Random.Range(-recoilRotationStrength, recoilRotationStrength),
                Random.Range(-recoilRotationStrength, recoilRotationStrength),
                0f
            );
            currentRecoilEuler += randomKick;
        }

        // 発射音
        if (shotSE != null)
            audioSource.PlayOneShot(shotSE);
    }
}
