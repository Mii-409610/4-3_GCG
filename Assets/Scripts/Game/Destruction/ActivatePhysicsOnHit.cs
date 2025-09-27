using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer; // Effekseerを使うため追加

/// <summary>
/// オブジェクト破壊するためのクラス
/// </summary>
public class ActivatePhysicsOnHit : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool hasScored = false; // スコア加算済みフラグ

    [HideInInspector] public float destroyNeighborRadius;
    [HideInInspector] public float explosionForce;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public float randomTorque;
    [HideInInspector] public float randomForce;
    [HideInInspector] public float blockDestroyDelay;
    [HideInInspector] public float bulletDestroyDelay;
    [HideInInspector] public EffekseerEffectAsset destroyEffect;
    [HideInInspector] public float effectScale = 1.0f;
    [HideInInspector] public float effectDuration = 1.0f;

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
            Vector3 hitPos = collision.contacts[0].point;

            // 範囲破壊
            if (destroyNeighborRadius > 0f)
            {
                Collider[] neighbors = Physics.OverlapSphere(hitPos, destroyNeighborRadius);

                foreach (var neighbor in neighbors)
                {
                    // ActivatePhysicsOnHitがついているブロックのみ
                    var apoh = neighbor.GetComponent<ActivatePhysicsOnHit>();
                    if (apoh != null && apoh != this)
                    {
                        apoh.BreakNeighborBlock(hitPos);
                    }
                }
            }

            // 破壊処理
            BreakNeighborBlock(hitPos);

            // エフェクト再生
            if (destroyEffect != null)
            {
                EffekseerHandle handle = EffekseerSystem.PlayEffect(destroyEffect, hitPos);
                handle.SetScale(new Vector3(effectScale, effectScale, effectScale));

                // 表示時間を制御
                if (effectDuration > 0f)
                {
                    StartCoroutine(StopEffectAfterDelay(handle, effectDuration));
                }
            }
            // 弾削除
            StartCoroutine(DestroyBulletAfterDelay(collision.gameObject));
        }
    }

    public void BreakNeighborBlock(Vector3 explosionPos)
    {
        if (rb != null)
        {
            // 物理挙動を有効化
            rb.isKinematic = false;
            rb.useGravity = true;

            // コライダーを無効化
            if (col != null)
            {
                col.enabled = false;
            }

            // 力を加える
            rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 0.5f, ForceMode.Impulse);

            // ランダムな力と回転を加える
            rb.AddForce(Random.onUnitSphere * randomForce, ForceMode.Impulse);

            // 回転速度の上限を設定
            rb.maxAngularVelocity = 100f;

            // 回転を加える
            rb.AddTorque(Random.insideUnitSphere * randomTorque, ForceMode.Impulse);

            // スコア加算処理
            if ((!hasScored && ScoreManager.Instance != null))
            {
                ScoreManager.Instance.AddScore(1);
                hasScored = true;
            }

            // オブジェクトを削除
            Destroy(gameObject, blockDestroyDelay);
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

    // エフェクトを一定時間で止める
    private IEnumerator StopEffectAfterDelay(EffekseerHandle handle, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handle.exists)
        {
            handle.Stop();
        }
    }
}