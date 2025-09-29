using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 地面設置用クラス
/// </summary>
// デバック描画用
//[ExecuteAlways]
public class MapManager : MonoBehaviour
{
    [SerializeField,Header("道路")]
    GameObject RoadObject;

    [SerializeField, Header("土地")]
    GameObject[] LandObjects;

    private void Awake()
    {
        if (RoadObject == null)
        {
            Debug.LogError("生成する道路オブジェクトが指定されていません。");
        }

        if (LandObjects == null || LandObjects.Length == 0)
        {
            Debug.LogError("生成する土地オブジェクトが指定されていません。");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(RoadObject);
        Instantiate(RoadObject, new Vector3(121.0f, 0.0f,   0.0f), Quaternion.identity);
        Instantiate(RoadObject, new Vector3(0.0f, 0.0f, 121.0f), Quaternion.identity);
        Instantiate(RoadObject, new Vector3(121.0f, 0.0f, 121.0f), Quaternion.identity);

        for (int i = 0; i < LandObjects.Length; i++)
        {
            Vector3 pos = Vector3.zero;
            switch (i)
            {
                case 0: pos = new Vector3(0.0f, 0.0f, 0.0f); break;
                case 1: pos = new Vector3(121.0f, 0.0f, 0.0f); break;
                case 2: pos = new Vector3(0.0f, 0.0f, 121.0f); break;
                case 3: pos = new Vector3(121.0f, 0.0f, 121.0f); break;
                default: pos = new Vector3(i * 10.0f, 0.0f, 0.0f); break; // 例
            }
            Instantiate(LandObjects[i], pos, Quaternion.identity);
        }
}

    // Update is called once per frame
    void Update()
    {

    }
}
