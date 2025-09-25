using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクト破壊するためのクラス
/// </summary>
public class ActivatePhysicsOnHit : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [HideInInspector] public float explosionForce;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public float randomTorque;
    [HideInInspector] public float randomForce;
    [HideInInspector] public float blockDestroyDelay;
    [HideInInspector] public float bulletDestroyDelay;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // bulletとの衝突判定
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (rb != null)
            {
                // 物理挙動を有効化
                rb.isKinematic = false;
                rb.useGravity = true;

                // コライダーを無効化
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

                // オブジェクトを削除
                Destroy(gameObject, blockDestroyDelay);
            }
            // 弾も削除
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