//=====マウスカーソルを取得用=====
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageIconHover : MonoBehaviour
{
    public bool isHovered = false;

    private Vector3 originalScale;
    public float pulseScaleAmount = 0.1f; // 拡縮の大きさ（±）
    public float pulseSpeed = 4.0f;       // 拡縮の速さ

    public float rotationSpeed = 30f; // 回転速度（度/秒）

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // マウス位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            isHovered = (hit.collider.gameObject == this.gameObject);
        }
        else
        {
            isHovered = false;
        }

        // 回転処理
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // 拡縮処理
        if (isHovered)
        {
            float scaleOffset = Mathf.Sin(Time.time * pulseSpeed) * pulseScaleAmount;
            transform.localScale = originalScale * (1.0f + scaleOffset);

        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 10f);
        }
    }
}