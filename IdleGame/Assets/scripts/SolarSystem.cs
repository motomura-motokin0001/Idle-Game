using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlanetData
{
    public GameObject TargetObject;
    public float Radius = 200f;
    
    // InfinitySystem.cs の URTS リスト内のどのインデックスに対応するか
    public int InfiIndex;

    [HideInInspector]
    public float currentAngle; 
}

public class SolarSystem : MonoBehaviour
{
    [SerializeField]
    private List<PlanetData> planetList = new List<PlanetData>();
    
    [SerializeField]
    public RectTransform centerPoint;

    [Header("演出設定")]
    [SerializeField]
    private double speedThreshold = 10.0; // 回転が止まって見える速度の閾値

    // リスト全体を取得して外部スクリプトで一括操作する場合用
    public List<PlanetData> GetPlanetList() => planetList;

    void Update()
    {
        // クラス名が InfinitySystem に変更されたことに対応
        if (InfinitySystem.instance == null) return;

        foreach (var planet in planetList)
        {
            if (planet.TargetObject == null) continue;

            // InfinitySystem 側のデータリストを取得
            var urtsList = InfinitySystem.instance.URTS;

            // インデックスが範囲内かチェック
            if (planet.InfiIndex >= 0 && planet.InfiIndex < urtsList.Count)
            {
                var planetStatus = urtsList[planet.InfiIndex];

                // レベルが0より大きい（動いている）ときのみ表示する
                bool shouldBeActive = planetStatus.level > 0;
                if (planet.TargetObject.activeSelf != shouldBeActive)
                {
                    planet.TargetObject.SetActive(shouldBeActive);
                }

                // 非表示の場合は計算をスキップ
                if (!shouldBeActive) continue;


                // 通常時：progress (0.0 ～ 1.0) を 360度に変換
                planet.currentAngle = (float)planetStatus.progress * 360f;
            }

            // 通常時の回転（真上 +90度オフセット）
            float displayAngle = planet.currentAngle + 90f;
            SetPlanetPosition(planet, displayAngle);
        }
    }

    /// <summary>
    /// 指定した角度に基づいて惑星の座標を更新する
    /// </summary>
    private void SetPlanetPosition(PlanetData planet, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * planet.Radius;
        float y = Mathf.Sin(radians) * planet.Radius;

        planet.TargetObject.transform.localPosition = 
            centerPoint.localPosition + new Vector3(x, y, 0);
    }
}