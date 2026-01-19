using UnityEngine;

[CreateAssetMenu(fileName = "BPD_Name", menuName = "Scriptable Objects/BeginningPlanetData")]
public class BeginningPlanetData : ScriptableObject
{
    public string Planet_Name;     // 惑星の名前
    public int Planet_ID;          // 惑星のID
    public double Level;           // 初期レベル
    public double Base_Cost;       // 惑星の基本コスト
    public double Cost_Multiplier; // 惑星のコスト倍率

    [Header("Production Settings")]
    public double Base_Speed = 0.2f;      // 基本移動速度
    public double Base_Reward = 1.0f;     // 初期の生産量（報酬）
    public double Reward_Exponent_Add = 0.1f; // 1周ごとの指数増加量
    public double Reward_Base_Value = 1.0f;   // 報酬計算の底 (Math.Pow(Base, Exponent))
    public double Ascension_Mult = 1.5f;      // アセンションごとの倍率
}