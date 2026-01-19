using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField]
    private double sampleInt;
    [SerializeField]
    public RectTransform centerPoint; // 中心のターゲット（空のGameObjectなど）
    public RectTransform tragetPoint; // 回転させるオブジェクト
    public float radius = 200f;       // 半径
    public float duration = 2f;       // 一周の時間

    private float currentAngle = 0f;

    void Update()
    {
        if (duration <= 0) return;

        // 1秒間に進む角度を計算 (360度 / 一周の時間)
        float speed = 360f / duration;
        currentAngle += speed * Time.deltaTime;

        // 座標計算
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * radius;
        float y = Mathf.Sin(radians) * radius;

        tragetPoint.localPosition = centerPoint.localPosition + new Vector3(x, y, 0);
    }
}
