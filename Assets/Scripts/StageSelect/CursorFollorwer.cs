using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorFollorwer : MonoBehaviour
{
    public RectTransform followTarget; // 追従する画像（RectTransform）
    public Image cursorImage;          // カーソルのImage（色を変える用）
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.red;

    void Start()
    {

    }

    void Update()
    {
        // 1. マウス位置にカーソルUIを移動
        Vector2 mousePosition = Input.mousePosition;
        followTarget.position = mousePosition;

        // 2. マウス位置からRayを飛ばす（カメラ前方向）
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 3. ヒットしたオブジェクトが"Stage1,2"なら色を変える
            if (hit.collider.CompareTag("Stage1")|| hit.collider.CompareTag("Stage2") || hit.collider.CompareTag("Title"))
            {
                cursorImage.color = hoverColor;
            }
            else
            {
                cursorImage.color = defaultColor;
            }
        }
        else
        {
            cursorImage.color = defaultColor;
        }
    }
}
