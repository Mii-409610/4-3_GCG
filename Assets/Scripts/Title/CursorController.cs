using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カーソルを動かすクラス
/// </summary>
public class CursorController : MonoBehaviour
{
    [SerializeField, Header("カーソルに使用するテクスチャ")]
    private Texture2D cursor;

    [SerializeField, Header("弾のプレハブ")]
    private GameObject bulletPrefab;

    [SerializeField, Header("弾の発射位置")]
    private Transform firePoint;

    [SerializeField, Header("弾のスピード")]
    private float bulletSpeed = 20.0f;


    void Awake()
    {
        // エラー処理
        if (cursor == null)
        {
            Debug.LogError("カーソルに使用するテクスチャが設定されていません");
            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("弾のプレハブが設定されていません");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("弾の発射位置が設定されていません");
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // カーソルを中心に合わせる
        //Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Shot();
        }

    }

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    void Shot()
    {
        // カメラの中央からレイを取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 弾の生成
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 弾の大きさを変更
        bullet.transform.localScale = Vector3.one * 3.5f;

        // 弾のRigidbodyを取得してレイの方向い力を加える
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.velocity = ray.direction * bulletSpeed;
        }

        Destroy(bullet, 3.0f);
    }
}
