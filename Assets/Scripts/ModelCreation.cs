using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCreation : MonoBehaviour
{
    [SerializeField, Header("ブロック数")]
    public int xCount = 0;
    public int yCount = 0;
    public int zCount = 0;

    [SerializeField, Header("オブジェクト名")]
    public string cubesParentName = "BlockGrid";

    [SerializeField, Header("Cubeのプレハブ")]
    public GameObject cubePrefab;

    private GameObject cubesParent;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBlockGrid();

        // 自身を削除
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateBlockGrid()
    {
        if(cubePrefab == null)
        {
            Debug.LogError("Cubeのプレハブが設定されていません。");
            return;
        }

        // 親オブジェクトを作成
        cubesParent = new GameObject(cubesParentName);

        // グリッドの原点(左下手前)を算出(中心座標基準)
        Vector3 origin = transform.position - new Vector3(
            (xCount - 1) * 0.5f,
            (yCount - 1) * 0.5f,
            (zCount - 1) * 0.5f
        );

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Vector3 pos = origin + new Vector3(x, y, z);
                    GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity, cubesParent.transform);
                    cube.name = $"Cube_{x}_{y}_{z}";
                }
            }
        }
    }
}
