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

        //// モーションブラーON
        //if (postProcessVolume != null)
        //{
        //    postProcessVolume.weight = 1.0f;
        //    //Debug.Log("ズーム開始");
        //}

        FindObjectOfType<ZoomEffect>().ShowSpeedLines();

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        Vector3 direction = (transform.position - startPos).normalized;
        Vector3 targetPos = transform.position - direction * zoomDistance;
        Quaternion targetRot = Quaternion.LookRotation(transform.position - targetPos);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        ////テスト
        //Time.timeScale = 0;
        //yield break;

        //yield return new WaitForSeconds(0.5f);

        // モーションブラーOFF（演出終了後）
        //if (postProcessVolume != null)
        //    postProcessVolume.weight = 0f;

        // ステージに応じてシーンを読み込み
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