using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject LandObject;

    private void Awake()
    {
        if (RoadObject == null)
        {
            Debug.LogError("生成する道路オブジェクトが指定されていません。");
        }

        if (LandObject == null)
        {
            Debug.LogError("生成する土地オブジェクトが指定されていません。");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(RoadObject);
        Instantiate(LandObject);
        Instantiate(RoadObject, new Vector3(121.0f, 0.0f,   0.0f), Quaternion.identity);
        Instantiate(LandObject, new Vector3(121.0f, 0.0f,   0.0f), Quaternion.identity);
        Instantiate(RoadObject, new Vector3(  0.0f, 0.0f, 121.0f), Quaternion.identity);
        Instantiate(LandObject, new Vector3(  0.0f, 0.0f, 121.0f), Quaternion.identity);
        Instantiate(RoadObject, new Vector3(121.0f, 0.0f, 121.0f), Quaternion.identity);
        Instantiate(LandObject, new Vector3(121.0f, 0.0f, 121.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
