using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

/// <summary>
/// 指定したブロック数で各軸ごとに分割するクラス
/// </summary>
public class BlockSlicer : MonoBehaviour
{
    [System.Serializable]
    public struct AxisSliceSetting
    {
        [Header("スライスの軸方向(X,Y,Z)")]
        public Vector3 axis;

        [Header("この軸での分割したいブロック数")]
        public int blockCount;
    }

    [SerializeField, Header("切断面のマテリアル")]
    private Material sliceMaterial;

    [SerializeField, Header("各軸ごとの分割設定")]
    private AxisSliceSetting[] sliceSettings;

    [SerializeField, Header("分割ブロックの質量")]
    private float blockMass = 300.0f;

    // 分割対象オブジェクトリスト
    private List<GameObject> sliceSourceObjectList;

    // 分割後のパーツをまとめる親オブジェクト
    private GameObject slicedObjectParent;

    private void Awake()
    {
        // 分割設定がなければ警告
        if ((sliceSettings == null || sliceSettings.Length == 0))
        {
            Debug.LogWarning("分割設定がありません。切断されません。");
        }

        // 分割対象リスト初期化
        sliceSourceObjectList = new List<GameObject>();
    }

    void Start()
    {
        // 分割対象に親オブジェクトを追加
        sliceSourceObjectList.Add(gameObject);

        // 分割処理
        SliceObject();

        // 親オブジェクト作成と階層化
        SetupSliceObjectsHierarchy();

        // 1ブロックのみなら元オブジェクトを消さない
        bool actuallySliced = false;
        foreach (var setting in sliceSettings)
        {
            if (setting.blockCount > 1)
            {
                actuallySliced = true;
                break;
            }
        }
        if (actuallySliced)
        {
            // 元オブジェクト削除
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 分割処理
    /// </summary>
    private void SliceObject()
    {
        // 分割基準となるBoxCollider（親オブジェクトから一度だけ取得）
        BoxCollider box = gameObject.GetComponent<BoxCollider>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider>();
        }

        // BoxColliderの中心とサイズはローカル座標で取得
        Vector3 boxCenter = box.center;
        Vector3 boxSize = box.size;

        // 各軸ごとに分割処理
        foreach (var setting in sliceSettings)
        {
            int blockCount = Mathf.Max(1, setting.blockCount); // 1未満は1に補正
            if (blockCount <= 1) continue; // 1以下の場合は分割しない

            int sliceCount = blockCount - 1; // 分割面の数

            // 次の軸での分割に備えて新しいリストを用意
            List<GameObject> nextSourceList = new List<GameObject>();

            // 現在の分割対象ブロックを順番に分割
            foreach (GameObject block in sliceSourceObjectList)
            {
                // 分割後のブロックリスト
                List<GameObject> dividedBlocks = new List<GameObject> { block };

                for (int i = 1; i <= sliceCount; i++)
                {
                    float ratio = (float)i / blockCount;

                    // 分割面の法線（ローカル座標系で取得）
                    Vector3 localNormal = setting.axis.normalized;
                    Vector3 worldNormal = block.transform.TransformDirection(localNormal);

                    // 分割面のローカル座標位置
                    Vector3 slicePosLocal;
                    if (Mathf.Abs(localNormal.x) > 0.99f)
                    {
                        // X軸分割
                        float xStart = boxCenter.x - boxSize.x / 2f;
                        float xPos = xStart + boxSize.x * ratio;
                        slicePosLocal = new Vector3(xPos, boxCenter.y, boxCenter.z);
                    }
                    else if (Mathf.Abs(localNormal.y) > 0.99f)
                    {
                        // Y軸分割
                        float yStart = boxCenter.y - boxSize.y / 2f;
                        float yPos = yStart + boxSize.y * ratio;
                        slicePosLocal = new Vector3(boxCenter.x, yPos, boxCenter.z);
                    }
                    else if (Mathf.Abs(localNormal.z) > 0.99f)
                    {
                        // Z軸分割
                        float zStart = boxCenter.z - boxSize.z / 2f;
                        float zPos = zStart + boxSize.z * ratio;
                        slicePosLocal = new Vector3(boxCenter.x, boxCenter.y, zPos);

                        //Debug.Log("Z軸分割: " + slicePosLocal);
                    }
                    else
                    {
                        Debug.LogWarning("軸ベクトルが不正: " + localNormal);
                        continue;
                    }

                    // slicePosLocalを分割対象ブロックのワールド座標へ変換
                    Vector3 slicePosWorld = block.transform.TransformPoint(slicePosLocal);

                    // 新しい分割結果リスト
                    List<GameObject> newDivided = new List<GameObject>();
                    foreach (var obj in dividedBlocks)
                    {
                        SlicedHull hull = obj.Slice(slicePosWorld, worldNormal, sliceMaterial);

                        if (hull != null)
                        {
                            GameObject upper = hull.CreateUpperHull(obj, sliceMaterial);
                            GameObject lower = hull.CreateLowerHull(obj, sliceMaterial);

                            if (upper != null)
                            {
                                upper.transform.position = obj.transform.position;
                                upper.transform.rotation = obj.transform.rotation;
                                upper.transform.localScale = obj.transform.localScale;
                                newDivided.Add(upper);
                            }
                            if (lower != null)
                            {
                                lower.transform.position = obj.transform.position;
                                lower.transform.rotation = obj.transform.rotation;
                                lower.transform.localScale = obj.transform.localScale;
                                newDivided.Add(lower);
                            }
                            Destroy(obj);
                        }
                        else
                        {
                            newDivided.Add(obj);
                        }
                    }
                    dividedBlocks = newDivided;
                }
                nextSourceList.AddRange(dividedBlocks);
            }
            sliceSourceObjectList = nextSourceList;
        }
    }

    /// <summary>
    /// 親オブジェクト作成・階層化・物理コンポーネント追加
    /// </summary>
    private void SetupSliceObjectsHierarchy()
    {
        // 分割後のパーツをまとめる親オブジェクト作成
        slicedObjectParent = new GameObject("SlicedObjects");
        slicedObjectParent.transform.position = transform.position;

        int idx = 0;
        foreach (var obj in sliceSourceObjectList)
        {
            obj.name = $"Block_{++idx}";

            // MeshCollider追加
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.enabled = true;

            // Rigidbody追加
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            // 分割ブロックの質量設定
            rb.mass = blockMass;

            // 衝突時に物理挙動を有効化するコンポーネント追加
            obj.AddComponent<ActivatePhysicsOnHit>();

            // 親オブジェクトの子に設定
            obj.transform.SetParent(slicedObjectParent.transform);

            // 0.1秒後に物理挙動を有効化
            StartCoroutine(EnablePhysicsDelayed(obj, collider, rb, 0.1f));
        }
    }

    /// <summary>
    /// 遅延して物理挙動を有効化
    /// </summary>
    IEnumerator EnablePhysicsDelayed(GameObject obj, MeshCollider collider, Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (collider != null) collider.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }
}