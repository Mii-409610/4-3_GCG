using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// MiniGunのクラス
/// </summary>
public class BulletMiniGun : MonoBehaviour
{
    [SerializeField, Header("銃口")]
    public Transform shootPoint;

    [SerializeField, Header("弾のモデル")]
    public GameObject bullet;

    [SerializeField, Header("発射の威力")]
    public float shootForce;

    //[SerializeField, Header("弾の発射間隔")]
    //public float fireInterval = 0.2f;

    [SerializeField, Header("弾の消える時間")]
    public float deleteTime = 0.0f;

    // レイヤー
    private LayerMask ignoreLayer;

    private GameObject playerCam;

    // 最後に弾を発射した時間
    //private float lastFireTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // カメラをシーン内から取得
        playerCam = GameObject.Find("PlayerCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // 挙動を一時停止
        if (IsGameManager.isGameEnded) return;

        // スペースキーが押されている間
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // カメラの中央からレイを飛ばす（画面中央のターゲット位置を取得）
            Ray ray = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            Vector3 targetPoint;

            // レイが何かに当たったら、その位置をターゲットにする
            if (Physics.Raycast(ray, out hit, 10000f, ~ignoreLayer))
            {
                targetPoint = hit.point;
            }
            else
            {
                // 何も当たらなければ適当な距離で決定
                targetPoint = ray.GetPoint(1000);
            }

            // 銃口からターゲットへの直線ベクトルを取得
            Vector3 directionWithoutSpread = targetPoint - shootPoint.position;

            // 弾を生成（位置は銃口）
            GameObject currentBullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);

            // 弾の向きをターゲット方向に設定
            currentBullet.transform.forward = directionWithoutSpread.normalized;

            // 弾に力を加えて飛ばす（Impulseモード＝瞬間的な力）
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

            // 弾の消滅
            Destroy(currentBullet, deleteTime);
        }
    }
}
