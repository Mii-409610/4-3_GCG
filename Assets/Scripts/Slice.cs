using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System;

[Serializable]
public struct Cut
{
    public int row, col;
}

[Serializable]
public class ObjectSliceData
{
    public string name;
    public Cut cut;
};

public class Slice : MonoBehaviour
{
    [SerializeField]
    private List<ObjectSliceData> data;

    [SerializeField, Header("スライスするオブジェクトのプレハブ")]
    private GameObject sliceObject;

    [SerializeField,Header("断面に使用するマテリアル")]
    private Material sliceMaterial;

    [SerializeField, Header("スライスする回数")]
    private int sliceCount;

    [SerializeField, Header("スライスの軸方向(X,Y,Z)")]
    Vector3[] sliceAxes;

    [SerializeField, Header("Rigidbodyコンポーネント追加の有無")]
    private bool addRigidbody;

    //切断オブジェクト
    private SlicedHull slicedHulls;

    // 元のオブジェクトリスト
    private List<GameObject> sliceSourceObjectList;

    // 切断後のオブジェクトリスト
    private List<GameObject> sliceTragetObjectList;

    // エラーが発生したかどうか
    private bool errorFlag;

    private void Awake()
    {
        // ========================================
        //  エラー処理
        // ========================================
        if(sliceObject == null)
        {
            Debug.LogError("切断するオブジェクトが設定されていません。");
            errorFlag = true;
            return;
        }

        if (sliceCount <= 0)
        {
            Debug.LogWarning("スライス回数が0なので、切断されません。");
        }

        if (sliceAxes.Length <= 0)
        {
            Debug.LogWarning("スライスの軸方向が設定されていないため、切断されません。");
        }

        // ========================================
        //  事前処理
        // ========================================
        sliceSourceObjectList = new List<GameObject>();
        sliceTragetObjectList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // エラーが発生している場合は処理を中断
        if (errorFlag) return;

        // 元のオブジェクトをリストに追加
        sliceSourceObjectList.Add(Instantiate(sliceObject));

        // ========================================
        //  切断処理
        // ========================================
        foreach(var axis in sliceAxes)
        {
            // 各軸を正規化
            axis.Normalize();

            for(int i = 0; i < sliceCount; i++)
            {
                // 指定した軸に沿ってオブジェクトをスライス
                SliceObject(axis);
            }
        }

        // ========================================
        //  切断後オブジェクトの親オブジェクト作成と階層化
        // ========================================

        // 切断後のオブジェクトをまとめる親オブジェクトを作成
        GameObject slicedObjectParent = new GameObject("SlicedObjects");

        foreach(var obj in sliceSourceObjectList)
        {
            // 各オブジェクトにMeshColliderを追加
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.convex = true;     // メッシュコライダーを凸形状に設定
            collider.enabled = false;   // 初期状態では無効化

            // Rigidbodyを追加する場合
            Rigidbody rb = null;
            if(addRigidbody)
            {
                rb = obj.AddComponent<Rigidbody>();
                rb.useGravity = false;  // 重力を無効化
                rb.isKinematic = true;  // 初期状態ではキネマティックに設定
            }
            obj.transform.SetParent(slicedObjectParent.transform);

            // 0.1秒後に有効化
            StartCoroutine(EnablePhysicsDelayed(obj, collider, rb, 0.1f));
        }

        // 元のオブジェクトリストをクリア
        sliceSourceObjectList.Clear();
    }

    /// <summary>
    /// 指定した軸に沿って、元オブジェクトリスト内の各オブジェクトをスライス
    /// </summary>
    /// <param name="sliceAxis">スライスに使用する軸方向</param>
    private void SliceObject(Vector3 sliceAxis)
    {
        foreach(var sliceObject in sliceSourceObjectList)
        {
            // オブジェクトの中心位置を取得
            Vector3 center = sliceObject.GetComponent<Renderer>().bounds.center;

            // オブジェクトをスライス
            slicedHulls = sliceObject.Slice(position: center, direction: sliceAxis);

            if(slicedHulls != null)
            {
                // スライスが成功した場合、上部ハルと下部ハルを生成
                GameObject upperHull = slicedHulls.CreateUpperHull(sliceObject, sliceMaterial);
                GameObject lowerHull = slicedHulls.CreateLowerHull(sliceObject, sliceMaterial);

                // 元のオブジェクトを削除
                Destroy(sliceObject);

                if(upperHull != null)
                {
                    upperHull.name = (sliceTragetObjectList.Count + 1).ToString();
                    // 上部ハルを切断後のリストに追加
                    sliceTragetObjectList.Add(upperHull);
                }

                if(lowerHull != null)
                {
                    lowerHull.name = (sliceTragetObjectList.Count + 1).ToString();
                    // 下部ハルを切断後のリストに追加
                    sliceTragetObjectList.Add(lowerHull);
                }
            }
            else
            {
                Debug.LogWarning("スライスに失敗しました。オブジェクト：" + sliceObject.name);
            }
        }
        sliceSourceObjectList.Clear();                          // 元のオブジェクトリストをクリア
        sliceSourceObjectList.AddRange(sliceTragetObjectList);  // 切断後のオブジェクトを次のスライスのソースとして使用
        sliceTragetObjectList.Clear();                          // 切断後のオブジェクトリストをクリア
    }

    /// <summary>
    /// コライダーとリジッドボディを遅延有効化するコルーチン
    /// </summary>
    /// <param name="obj">対象のゲームオブジェクト</param>
    /// <param name="collider">対象のMeshCollider</param>
    /// <param name="rb">対象のRigidbody</param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator EnablePhysicsDelayed(GameObject obj, MeshCollider collider, Rigidbody rb, float delay)
    {
        // 指定した時間待機
        yield return new WaitForSeconds(delay);

        if (collider != null) collider.enabled = true;  // コライダーを有効化
        if (rb != null) rb.isKinematic = false;         // リジッドボディを非キネマティックに設定
    }
}
