using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("弾のモデル")]
    public GameObject bullet;

    [SerializeField, Header("弾の速さ")]
    public float speed = 0.0f;

    [SerializeField, Header("弾の発射間隔")]
    public float fireInterval = 0.2f;

    [SerializeField, Header("弾の消える時間")]
    public float deleteTime = 0.0f;

    // 最後に弾を発射した時間
    private float lastFireTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーが押されている間
        if(Input.GetKey(KeyCode.Space))
        {
            // 最後の発射から一定時間たっていれば発射可能
            if(Time.time - lastFireTime >= fireInterval)
            {
                // カメラオブジェクトを取得
                GameObject camera = GameObject.Find("Main Camera");

                // カメラの前方向を取得
                Vector3 camForward = camera.transform.forward;

                // 弾を出す位置を計算
                Vector3 pos = transform.position;
                pos.y += 0.0f;
                pos.z += 0.1f;
                pos += camForward * 1.0f; 

                // 弾のプレハブをインスタンス化
                GameObject copy = Instantiate(
                    bullet,             // 弾のプレハブ
                    pos,                // 出現させる位置
                    Quaternion.identity // 向きを固定
                    );

                // 弾のRigidbodyを取得
                Rigidbody rb = copy.GetComponent<Rigidbody>();

                // 前方向にスピードを掛ける
                camForward = camForward * speed;

                // Impulse(瞬間的な力)として前に飛ばす
                rb.AddForce(camForward, ForceMode.Impulse);

                // 弾の消滅
                Destroy(copy, deleteTime);

                // 最終発射時間を更新
                lastFireTime = Time.time;
            }
        }
    }
}
