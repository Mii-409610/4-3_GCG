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

    [SerializeField, Header("リロード設定")]
    public float reloadTime = 2.0f;     // リロード所要時間(秒)
    
    [SerializeField, Header("UI")]
    public Image reloadGauge;           // リロードゲージのUI

    [SerializeField, Header("効果音")]
    public AudioClip shotSE;            // 発射音
    private AudioSource audioSource;    // 効果音再生用

    [SerializeField, Header("無視するレイヤー")]
    public LayerMask ignoreLayer;       // Raycast時に無視するレイヤー

    // ======= 内部状態 =======
    private int currentBullet;  // 現在の弾数
    private string weaponName;  // 武器名(リロード管理用)


    void Start()
    {
        // 武器名を設定
        weaponName = gameObject.name;

        // 弾数を最大にセット
        currentBullet = maxCapacity;

        // AudioSourceを取得(なければ追加)
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // リロードゲージを満タンに初期化
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1.0f;
    }

    void Update()
    {
        // 発射処理(弾が残っているときのみ)
        if (Input.GetKeyDown(KeyCode.Space) && currentBullet > 0)
        {
            FireLauncher();
        }

        // 弾切れ時リロード開始
        if (currentBullet <= 0 && !WeaponReloadManager.IsReloading(weaponName))
        {
            WeaponReloadManager.StartReload(
                weaponName, // 武器名
                reloadTime, // リロード時間
                () =>
                {    
                    // リロード完了時の処理
                    currentBullet = maxCapacity;                            // 弾数を最大に戻す
                    if (reloadGauge != null) reloadGauge.fillAmount = 1.0f; // ゲージ満タン
                },
                reloadGauge // リロードゲージUI
            );
        }

        // リロード中UI進捗
        if (WeaponReloadManager.IsReloading(weaponName))
        {
            // ゲージの進捗を更新
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

        // 弾の消滅
        Destroy(copy, deleteTime);

        // 発射音があれば再生
        if (shotSE != null)
            audioSource.PlayOneShot(shotSE);
    }
}
