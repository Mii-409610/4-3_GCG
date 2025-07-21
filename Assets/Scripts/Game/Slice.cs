using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

/// <summary>
/// 軸ごとにスライス回数を指定してオブジェクトを分割するクラス
/// </summary>
public class Slice : MonoBehaviour
{
    [System.Serializable]
    public struct AxisSliceSetting
    {
        [Header("スライスの軸方向(X,Y,Z)")]
        public Vector3 axis;

        [Header("この軸でのスライス回数")]
        public int sliceCount;
    }

    [SerializeField,Header("切断面のマテリアル")]
    private Material sliceMaterial;

    [SerializeField, Header("各軸ごとのスライス設定")]
    private AxisSliceSetting[] sliceSettings;

    //切断オブジェクト
    private SlicedHull slicedHulls;

    // スライス対象と結果のリスト
    private List<GameObject> sliceSourceObjectList;
    private List<GameObject> sliceTragetObjectList;

    private void Awake()
    {
        // ========================================
        //  エラー処理
        // ========================================
        if ((sliceSettings == null || sliceSettings.Length == 0))
        {
            Debug.LogWarning("スライス設定がありません。切断されません。");
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
        // 自分自身をスライス対象に追加
        sliceSourceObjectList.Add(gameObject);

        // ========================================
        //  切断処理
        // ========================================
        foreach (var setting in sliceSettings)
        {
            Vector3 normAxis = setting.axis.normalized;

            for (int i = 0; i < setting.sliceCount; i++)
            {
                SliceObject(normAxis);
            }
        }

        // ========================================
        //  切断後オブジェクトの親オブジェクト作成と階層化
        // ========================================

        // 切断後のオブジェクトをまとめる親オブジェクトを作成
        GameObject slicedObjectParent = new GameObject("SlicedObjects");
        slicedObjectParent.transform.position = transform.position;

        // コルーチンをすべてを待機するためのリスト
        List<Coroutine> delayedCoriutines = new List<Coroutine>();

        foreach(var obj in sliceSourceObjectList)
        {
            // MEMO: コルーチン合っても無くてもどっちでもいける（ようわからん）
            // 各オブジェクトにMeshColliderを追加
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.convex = true;     // メッシュコライダーを凸形状に設定
            collider.enabled = true;   // 初期状態では無効化

            // Rigidbodyを追加する場合
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = false;  // 重力を無効化
            rb.isKinematic = true;  // 初期状態ではキネマティックに設定

            obj.AddComponent<ActivatePhysicsOnHit>();

            // 親オブジェクトの子に設定
            obj.transform.SetParent(slicedObjectParent.transform);

            // 0.1秒後に有効化
            StartCoroutine(EnablePhysicsDelayed(obj, collider, rb, 0.1f));
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// 軸に沿って、各オブジェクトをスライス
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

            if (slicedHulls != null)
            {
                GameObject upperHull = slicedHulls.CreateUpperHull(sliceObject, sliceMaterial);
                GameObject lowerHull = slicedHulls.CreateLowerHull(sliceObject, sliceMaterial);

                Destroy(sliceObject);

                if (upperHull != null)
                {
                    upperHull.name = (sliceTragetObjectList.Count + 1).ToString();
                    // 上部ハルを切断後のリストに追加
                    sliceTragetObjectList.Add(upperHull);
                }

                if (lowerHull != null)
                {
                    lowerHull.name = (sliceTragetObjectList.Count + 1).ToString();
                    // 下部ハルを切断後のリストに追加
                    sliceTragetObjectList.Add(lowerHull);
                }
            }
            else
            {
                Debug.LogWarning("スライスに失敗: " + sliceObject.name);
            }
        }
        sliceSourceObjectList.Clear();                          // 元のオブジェクトリストをクリア
        sliceSourceObjectList.AddRange(sliceTragetObjectList);  // 切断後のオブジェクトを次のスライスのソースとして使用
        sliceTragetObjectList.Clear();                          // 切断後のオブジェクトリストをクリア
    }

    /// <summary>
    /// 遅延して物理挙動を有効化
    /// </summary>
    /// <param name="obj">対象のゲームオブジェクト</param>
    /// <param name="collider">対象のMeshCollider</param>
    /// <param name="rb">対象のRigidbody</param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator EnablePhysicsDelayed(GameObject obj, MeshCollider collider, Rigidbody rb, float delay)
    {
        Debug.Log("コルーチン開始");

        // 指定した時間待機
        yield return new WaitForSeconds(delay);

        if (collider != null) collider.enabled = true;  // コライダーを有効化
        if (rb != null) rb.isKinematic = false;         // リジッドボディを非キネマティックに設定
    }
}
