using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering.PostProcessing;//モーションブラーに必要

public class StageSelect : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 2.0f;
    public float zoomDistance = 3.0f;

   // public PostProcessVolume postProcessVolume; 

    private bool isZooming = false;

    public enum StageType
    {
        //ステージセレクトの列挙型
        Stage1,
        Stage2,
        
    }

    public StageType stageType; // ← Inspectorで選べる

    public void TriggerZoom()
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomAndLoad());
        }
    }

    IEnumerator ZoomAndLoad()
    {
        isZooming = true;

        FindObjectOfType<ZoomEffect>().ShowSpeedLines();

        Vector3 startPos = mainCamera.transform.position;

        // カメラの正面方向（向きを変えない）
        Vector3 direction = mainCamera.transform.forward;

        // アイコンが中央に来るような位置へ調整
        // → アイコンからカメラの正面方向に zoomDistance だけ離れた位置へ移動
        Vector3 targetPos = transform.position - direction * zoomDistance;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        switch (stageType)
        {
            case StageType.Stage1:
                FindObjectOfType<SelectFadeOut>().StartSceneTransition("Debug");
                break;
            case StageType.Stage2:
                FindObjectOfType<SelectFadeOut>().StartSceneTransition("Debug");
                break;
            default:
                Debug.LogWarning("未定義のステージです");
                break;
        }
    }
}