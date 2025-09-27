using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionSwitcher : MonoBehaviour
{
    [SerializeField, Header("破壊用分割オブジェクト")]
    public GameObject dividedObject;

    private Rigidbody rb;
    private Collider col;
    private bool isDestroyed = false;

    void Awake()
    {
        if(dividedObject == null)
        {
            Debug.LogError("DestructionSwitcher: dividedObjectが設定されていません。");
            return;
        }
    }

    void Start()
    {
        // Rigidbody追加
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Collider追加
        col = GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isDestroyed && collision.gameObject.CompareTag("Bullet"))
        {
            isDestroyed = true;

            // 親オブジェクトを取得
            Transform parentTrans = transform.parent != null ? transform.parent : transform;

            // 親ごと削除
            Destroy(parentTrans.gameObject);

            // 分割オブジェクトを生成
            if (dividedObject != null)
            {
                GameObject obj = Instantiate(
                    dividedObject,
                    parentTrans.position,
                    parentTrans.rotation);

                obj.transform.localScale = parentTrans.localScale;
            }
        }
    }
}
