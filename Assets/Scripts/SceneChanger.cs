using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン切り替えのクラス
/// </summary>
public class SceneChanger : MonoBehaviour
{
    // 遷移先のシーン名
    //private string nextSceneName = "Debug";

    void Start()
    {

    }

    /// <summary>
    /// シーンを切り替える共通関数
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
