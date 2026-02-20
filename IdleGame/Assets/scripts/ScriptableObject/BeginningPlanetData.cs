using UnityEngine;

[CreateAssetMenu(fileName = "BPD_Name", menuName = "Scriptable Objects/BeginningPlanetData")]
public class BeginningPlanetData : ScriptableObject
{
    public string Planet_Name;                  // 惑星の名前
    public int Planet_ID;                       // 惑星のID
    public double Level;                        // 初期レベル

    [Header("Production Settings")]
    public double Base_Cost;                    // 惑星の基本コスト
    public double Cost_Multiplier;              // 惑星のコストの増加量
    public double Base_Speed = 0.2f;            // 惑星の一周のスピード
    public double Ascension_Multiplier = 1.5f;  // 惑星のプレスステージの増加量
    public double Base_Reward = 1.0f;           // スコアの加算量
}