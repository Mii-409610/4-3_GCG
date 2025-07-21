using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ミニガンの弾を制御するクラス
/// </summary>
public class BulletMiniGun : MonoBehaviour, IWeaponControl
{
    // 銃口
    [SerializeField, Header("銃口")]
    public Transform shootPoint;

    // 弾の設定
    [SerializeField, Header("弾設定")]
    public GameObject bullet;           // 弾のプレハブ
    public float speed = 0.0f;          // 弾の速さ
    public float deleteTime = 0.0f;     // 弾の寿命(発射後に消えるまでの時間)
    public int maxCapacity = 5;         // 最大弾数

    // リロード設定
    [SerializeField, Header("リロード時間")]
    public float reloadTime = 2.0f;       // リロード時間（秒）
    private bool isReloading = false;   // リロード中フラグ

    // UI関連
    [SerializeField, Header("リロードゲージのUI")]
    public Image reloadGauge;

    // 効果音関連
    [SerializeField, Header("効果音")]
    public AudioClip shotSE;
    private AudioSource audioSource;

    // 現在弾数
    private int currentBullet;

    // 現在選択中の武器どうかのフラグ
    private bool isCurrentWeapon = false;

    // レイヤー(未使用？)
    private LayerMask ignoreLayer;

    // プレイヤーカメラ(未使用？)
    private GameObject playerCam;

    // Start is called before the first frame update
    void Start()
    {
        // 弾数を最大にセット
        currentBullet = maxCapacity;

        // AudioSourceを取得または追加
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // ゲージを満タンに
        if (reloadGauge != null)
            reloadGauge.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 現在の武器ではない、またはリロード中なら何もしない
        if (!isCurrentWeapon || isReloading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 弾切れなら発射しない
            if (currentBullet <= 0) return;
            Fire();
        }
    }

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    void Fire()
    {
        // 弾数を減らす
        currentBullet--;

        // カメラの前方方向を取得
        Vector3 dir = Camera.main.transform.forward;

        // 弾の発射位置を計算
        Vector3 pos = transform.position + dir * 1f + Vector3.up * 1.5f;

        GameObject b = Instantiate(bullet, pos, Quaternion.LookRotation(dir));
        b.GetComponent<Rigidbody>().velocity = dir * speed;
        Destroy(b, deleteTime);

        if (shotSE != null)
            audioSource.PlayOneShot(shotSE);

        if (currentBullet <= 0 && !isReloading)
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

        currentBullet = maxCapacity;
        isReloading = false;
        reloadGauge.fillAmount = 1f;
    }

    /// <summary>
    /// 武器の有効/無効を設定する
    /// </summary>
    /// <param name="isActive">有効かどうか</param>
    public void SetWeaponActive(bool isActive)
    {
        // 武器切り替え時に呼ばれる
        isCurrentWeapon = isActive;
    }
}
