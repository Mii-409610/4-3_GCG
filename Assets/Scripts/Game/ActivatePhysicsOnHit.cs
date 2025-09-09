using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePhysicsOnHit : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [SerializeField, Header("飛び散り設定")]
    public float explosionForce = 5.0f;     // 衝撃の強さ
    public float explosionRadius = 2.0f;    // 衝撃範囲
    public float randomTorque = 1.0f;       // 回転の強さ
    public float randomForce = 10.0f;       // ランダムな力の強さ

    [SerializeField, Header("Bullet消滅までの時間(秒)")]
    public float bulletDestroyDelay = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (rb == null)
        {
            Debug.LogWarning("[ActivatePhysicsOnHit] Rigidbody が見つかりませんでした。");
        }
        if (col == null)
        {
            Debug.LogWarning("[ActivatePhysicsOnHit] Collider が見つかりませんでした。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // bulletとの衝突判定
        if (collision.gameObject.CompareTag("Bullet")) // bullet はタグで判別
        {
            if (rb != null)
            {
                // 物理挙動を有効化
                rb.isKinematic = false;
                rb.useGravity = true;

                // 当たった瞬間にコライダーを無効化
                if(col != null)
                {
                    col.enabled = false;
                }

                // 爆発的な力を与える
                Vector3 explosionPos = collision.contacts[0].point;
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 0.5f, ForceMode.Impulse);

                // ランダムな力を加える
                Vector3 randDir = Random.onUnitSphere; // ランダム方向
                rb.AddForce(randDir * randomForce, ForceMode.Impulse);

                // ランダムな回転力を加える
                rb.maxAngularVelocity = 100f; // 最大角速度を増加
                Vector3 torque = new Vector3(
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque)
                );
                rb.AddTorque(torque, ForceMode.Impulse);
            }
            // 衝突した弾丸を削除
            StartCoroutine(DestroyBulletAfterDelay(collision.gameObject));
        }
    }

    private IEnumerator DestroyBulletAfterDelay(GameObject bulletObj)
    {
        yield return new WaitForSeconds(bulletDestroyDelay);
        if (bulletObj != null)
        {
            Destroy(bulletObj);
        }
    }
}