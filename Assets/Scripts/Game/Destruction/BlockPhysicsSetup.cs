using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer; // Effekseer用

/// <summary>
/// 破壊オブジェクトに物理挙動を設定するためのクラス
/// </summary>
public class BlockPhysicsSetup : MonoBehaviour
{
    [SerializeField, Header("範囲破壊の半径")]
    public float destroyNeighborRadius = 5.0f;

    [SerializeField, Header("飛び散り設定")]
    public float explosionForce = 1.0f;     // 衝撃の強さ
    public float explosionRadius = 0.5f;    // 衝撃範囲
    public float randomTorque = 0.3f;       // 回転の強さ
    public float randomForce = 1.0f;        // ランダムな力の強さ

    [SerializeField, Header("消滅までの時間")]
    public float blockDestroyDelay = 2.0f;
    public float bulletDestroyDelay = 0.5f;

    [SerializeField, Header("Effekseerエフェクト設定")]
    public EffekseerEffectAsset destroyEffect; // 破壊エフェクト
    public float effectScale = 1.0f;           // エフェクトの大きさ
    public float effectDuration = 2.0f;        // エフェクトの表示時間（秒）

    void Start()
    {
        foreach (Transform child in transform)
        {
            // Rigidbody追加
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            // Collider追加
            Collider col = child.GetComponent<Collider>();
            if (col == null)
            {
                child.gameObject.AddComponent<BoxCollider>();
            }

            // ActivatePhysicsOnHit追加＆パラメータ設定
            ActivatePhysicsOnHit apoh = child.GetComponent<ActivatePhysicsOnHit>();
            if (apoh == null)
            {
                apoh = child.gameObject.AddComponent<ActivatePhysicsOnHit>();
            }

            // BlockPhysicsSetupの値を子のActivatePhysicsOnHitに渡す
            apoh.destroyNeighborRadius = destroyNeighborRadius;
            apoh.explosionForce = explosionForce;
            apoh.explosionRadius = explosionRadius;
            apoh.randomTorque = randomTorque;
            apoh.randomForce = randomForce;
            apoh.blockDestroyDelay = blockDestroyDelay;
            apoh.bulletDestroyDelay = bulletDestroyDelay;
            apoh.destroyEffect = destroyEffect;
            apoh.effectScale = effectScale;
            apoh.effectDuration = effectDuration;
        }
    }
}
