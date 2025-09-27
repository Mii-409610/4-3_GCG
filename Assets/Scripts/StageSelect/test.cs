using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovered = false;

    // マウスが画像に乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // マウスが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void Update()
    {
        if (isHovered && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("stage1");
        }
    }
}
