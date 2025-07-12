using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shotgunのクラス
/// </summary>
public class BulletShotgun : MonoBehaviour
{
    [SerializeField, Header("弾のモデル")]
    public GameObject bullet;

    // public GameBalanceLoader balanceLoader; //JSONファイル読み込みのプレハブ

    [SerializeField, Header("弾の速さ")]
    public float speed = 0.0f; // 弾の速さ

    [SerializeField, Header("弾の消える時間")]
    public float deletetime = 0.0f;

    [SerializeField, Header("発射音")]
    public AudioClip shotSE;

    // 音を鳴らすためのAudioAource
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // AudioSourceを取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 挙動を一時停止
        if (IsGameManager.isGameEnded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // カメラオブジェクトを取得
            GameObject cam = GameObject.Find("PlayerCamera");

            // カメラ方向を取得
            Vector3 forward = cam.transform.forward;    // 正面
            Vector3 up = cam.transform.up;              // 上
            Vector3 right = cam.transform.right;        // 右

            // 発射位置の中心
            Vector3 basePos = transform.position + forward * 1.0f + Vector3.up * 1.5f;

            float offset = 0.2f; // 上下左右の間隔

            for (int y = -1; y <= 1; y++) // 上中下
            {
                for (int x = -1; x <= 1; x++) // 上中下の3発
                {
                    // ばらけた位置を計算
                    Vector3 spawnPos = basePos + up * y * offset + right * x * offset;

                    // 弾を生成
                    GameObject copy = Instantiate(bullet, spawnPos, Quaternion.LookRotation(forward));

                    // 弾のRigidbodyを取得
                    Rigidbody rb = copy.GetComponent<Rigidbody>();

                    // 前方向にスピードを掛ける
                    rb.velocity = forward * speed;

                    // 弾の消滅
                    Destroy(copy, deletetime);
                }

                // 発射音が設定されていれば再生
                if (shotSE != null)
                {
                    audioSource.PlayOneShot(shotSE);
                }
            }
        }
    }
}
