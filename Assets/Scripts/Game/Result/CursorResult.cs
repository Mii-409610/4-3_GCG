using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CursorResult : MonoBehaviour
{
    public RectTransform followTarget;
    public Image cursorImage;
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.red;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    public float scaleAmount = 1.1f;
    public float scaleSpeed = 4f;

    private RectTransform lastHoveredRect;
    public FadeOutResult fadeoutrisult;
    public float delayBeforeFade = 0.0f;

    // 遷移先のシーン定義
    private enum SceneToLoad
    {
        StageSelect,
        RestartCurrent,
        Stage1,
        Stage2,
        Stage3,
        None
    }

    // タグ名 → 遷移先Scene列挙型の辞書
    private Dictionary<string, SceneToLoad> tagToSceneMap = new Dictionary<string, SceneToLoad>()
    {
        { "StageSelect", SceneToLoad.StageSelect },
        { "ReStart", SceneToLoad.RestartCurrent },
        { "Stage1", SceneToLoad.Stage1 },
        { "Stage2", SceneToLoad.Stage2 },
        { "Stage3", SceneToLoad.Stage3 },
    };

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        followTarget.position = mousePos;

        PointerEventData pointerData = new PointerEventData(eventSystem) { position = mousePos };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        bool found = false;
        RectTransform currentHovered = null;
        string hoveredTag = "";

        foreach (RaycastResult result in results)
        {
            string tag = result.gameObject.tag;

            if (tagToSceneMap.ContainsKey(tag))
            {
                found = true;
                hoveredTag = tag;
                currentHovered = result.gameObject.GetComponent<RectTransform>();

                if (lastHoveredRect != null && lastHoveredRect != currentHovered)
                {
                    lastHoveredRect.localScale = Vector3.one;
                }

                if (currentHovered != null)
                {
                    float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * 0.05f;
                    float scale = scaleAmount + scaleOffset;
                    currentHovered.localScale = new Vector3(scale, scale, 1f);
                }

                lastHoveredRect = currentHovered;

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneToLoad sceneEnum = tagToSceneMap[tag];
                    string sceneName = ConvertEnumToSceneName(sceneEnum);

                    if (sceneEnum == SceneToLoad.StageSelect)
                    {
                        PlayerPrefs.SetInt("Stage1Cleared", 1);
                        PlayerPrefs.Save();
                    }

                    StartCoroutine(StartFadeAfterDelay(sceneName));
                }

                break;
            }
        }

        cursorImage.color = found ? hoverColor : defaultColor;
    }

    IEnumerator StartFadeAfterDelay(string sceneName)
    {
        yield return new WaitForSecondsRealtime(delayBeforeFade);
        fadeoutrisult.StartFade(sceneName);
    }

    // 列挙型 → 実際のシーン名に変換
    private string ConvertEnumToSceneName(SceneToLoad scene)
    {
        switch (scene)
        {
            case SceneToLoad.StageSelect: return "StageSelect";
            case SceneToLoad.RestartCurrent: return SceneManager.GetActiveScene().name;
            case SceneToLoad.Stage1: return "Stage1";
            case SceneToLoad.Stage2: return "Stage2";
            case SceneToLoad.Stage3: return "Stage3";
            default: return "";
        }
    }
}