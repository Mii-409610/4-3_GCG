using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public GameObject bullet;        // キャノン弾のプレハブ
    public float speed = 30f;        // 発射速度
    public float deleteTime = 5f;    // 弾が消えるまでの時間
    public float cooldown = 3f;      // クールタイム

    private float lastShotTime = -999f;

    public AudioClip shotSE;         // 発射音
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 左クリック or スペースキーで発射
        if (Input.GetKey(KeyCode.Space) && Time.time - lastShotTime >= cooldown)
        {
            FireCannon();
            lastShotTime = Time.time;
        }
    }

    void FireCannon()
    {
        // カメラの正面方向と位置を取得
        GameObject cam = GameObject.Find("PlayerCamera");
        Vector3 forward = cam.transform.forward;
        Vector3 spawnPos = transform.position + forward * 1.0f + Vector3.up * 1.5f;

        // 弾を生成
        GameObject copy = Instantiate(bullet, spawnPos, Quaternion.LookRotation(forward));
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        rb.velocity = forward * speed;

        // 一定時間後に自動で削除
        Destroy(copy, deleteTime);

        // SEを再生
        if (shotSE != null)
        {
            audioSource.PlayOneShot(shotSE);
        }
    }
}
