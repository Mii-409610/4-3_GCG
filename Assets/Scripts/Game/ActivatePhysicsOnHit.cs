using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePhysicsOnHit : MonoBehaviour
{
    private Rigidbody rb;

    [Header("飛び散り設定")]
    public float explosionForce = 8.0f;        // 衝撃の強さ
    public float explosionRadius = 2.0f;       // 衝撃範囲（小さめにすると自ブロック中心）
    public float randomTorque = 5.0f;          // 回転の強さ

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("[ActivatePhysicsOnHit] Rigidbody が見つかりませんでした。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // bullet はタグで判別
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                // 衝突した位置を中心に爆発的な力を加える
                Vector3 explosionPos = collision.contacts[0].point;
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 0.5f, ForceMode.Impulse);

                // ランダムな回転力を加える
                Vector3 torque = new Vector3(
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque)
                );
                rb.AddTorque(torque, ForceMode.Impulse);
            }
        }
    }
}