// using System;
// using System.Collections.Generic;
// using System.Text;
// using TMPro;
// using UnityEngine;

// public class Infi : MonoBehaviour
// {
//     public static Infi instance;

//     [Header("Data Settings")]
//     [SerializeField] private List<BeginningPlanetData> BPD_List; // 共通データ（ScriptableObject）

//     [Header("Economy & UI")]
//     public double currentScore; // 現在のスコア
//     [SerializeField] private TextMeshProUGUI totalScoreText; // 現在のスコアの表示用テキスト
//     [SerializeField] private TextMeshProUGUI multiplierText; // 乗数テキスト

//     [Header("Global Ascension Settings")]
//     public int baseAscensionGoal = 100; // 初期のアセンションに必要なレベル
//     public int levelIncrementPerAscension = 10; // アセンションをした際の次回のアセンション増加量

//     [Header("Runtime State")]
//     public List<UpgradeRuntimeState> URTS = new List<UpgradeRuntimeState>(); // 実行時のリスト（リアルタイム）

//     void Awake() // #Y初期化機能
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         foreach (var data in BPD_List) // 共通データをURTSに情報を追加
//         {
//             URTS.Add(new UpgradeRuntimeState(data));
//         }

//         // 初回起動時（またはスコアが0の場合）のみ初期スコアを配布
//         if (URTS.Count > 0 && currentScore <= 0)
//         {
//             currentScore = URTS[0].GetCurrentCost(); // 最初の惑星のコストと同じ数字にする
//         }
//     }

//     void Update() // #Y（毎フレーム）
//     {
//         // 1. 全惑星の倍率の「積」を計算し、スコアを増加させる
//         double totalMultiplier = CalculateTotalMultiplier();
//         currentScore += totalMultiplier * Time.deltaTime;

//         // 2. UI表示の更新
//         if (totalScoreText != null)
//         {
//             totalScoreText.text = "Total Score: " + FormatNumber(currentScore);
//         }

//         UpdateMultiplierDisplay(totalMultiplier); // 各惑星の乗数を式にしてテキストを更新

//         // 3. 各惑星の回転（指数の増加）を処理
//         foreach (var planet in URTS)
//         {
//             UpdatePlanetProgress(planet);
//         }
//     }

//     // 全ての惑星の生産倍率を掛け合わせる
//     private double CalculateTotalMultiplier()
//     {
//         double total = 1.0;
//         foreach (var p in URTS)
//         {
//             // 個別倍率 = (底 ^ 指数) * アセンション倍率
//             total *= p.data.Reward_Base_Value;
//         }
//         return total;
//     }

//     private void UpdateMultiplierDisplay(double total) // #Y各惑星の乗数の表示更新
//     {
//         if (multiplierText == null || URTS.Count == 0) return;

//         StringBuilder sb = new StringBuilder(); // StringBuilder は 「文字列を効率よく組み立てるための専用ツール」
//         for (int i = 0; i < URTS.Count; i++)
//         {
//             var p = URTS[i];
//             double currentProd = Math.Pow(p.data.Reward_Base_Value, p.rewardExponent) * Math.Pow(p.data.Ascension_Mult, p.ascensionCount);
            
//             sb.Append(FormatNumber(currentProd)); // 計算された値を短縮変換して追記

//             if (i < URTS.Count - 1)
//             {
//                 sb.Append(" × "); // 惑星の間に「×」を追記
//             }
//         }

//         // 合計の積（秒給）も表示
//         sb.Append("\n= Total: ").Append(FormatNumber(total));
//         multiplierText.text = sb.ToString(); // 完成式を表示
//     }

//     public int GetAscensionGoal(int index) // #Yアセンション目標レベルの取得
//     {
//         if (index < 0 || index >= URTS.Count)
//         {
//             return baseAscensionGoal;
//         }

//         return baseAscensionGoal + (URTS[index].ascensionCount * levelIncrementPerAscension);
//     }

//     private void UpdatePlanetProgress(UpgradeRuntimeState planet)
//     {
//         if (planet.level > 0)
//         {
//             // 回転速度の計算
//             double speedMult = Math.Pow(planet.data.Ascension_Mult, planet.ascensionCount);
//             double speed = planet.data.Base_Speed * planet.level * speedMult;
            
//             planet.progress += speed * Time.deltaTime;

//             // 100％（1.0）になったら
//             if (planet.progress >= 1.0)
//             {
//                 int completions = (int)Math.Floor(planet.progress); // 切り捨てで周回数を算出
//                 for (int i = 0; i < completions; i++)
//                 {
//                     // 1周するごとに指数が増え、全体の「積」が大きくなる
//                     planet.rewardExponent += planet.data.Reward_Exponent_Add;
//                 }
//                 planet.progress %= 1.0; // 余りを次に持ち越す
//             }
//         }
//     }

//     public void BuyUpgrade(int index)
//     {
//         if (index < 0 || index >= URTS.Count) return;
//         var planet = URTS[index];
//         double cost = planet.GetCurrentCost();
//         if (currentScore >= cost)
//         {
//             currentScore -= cost;
//             planet.level++;
//         }
//     }

//     public void PerformIndividualAscension(int index)
//     {
//         if (index < 0 || index >= URTS.Count) return;
//         var planet = URTS[index];
//         int goal = GetAscensionGoal(index);

//         if (planet.level >= goal)
//         {
//             planet.ascensionCount++;
//             planet.level = (int)planet.data.Level;
//             planet.progress = 0;
//             planet.rewardExponent = 0;
//             Debug.Log($"{planet.data.Planet_Name} Ascended!");
//         }
//     }

//     public string FormatNumber(double number)
//     {
//         if (number < 1.0) return number.ToString("F3");
//         if (number < 1000000) return number.ToString("F2");
//         return number.ToString("0.00e0");
//     }

//     [System.Serializable] // リアルタイムで変更ある変数
//     public class UpgradeRuntimeState
//     {
//         public BeginningPlanetData data;
//         public int level;
//         public double progress; // 進捗
//         public int ascensionCount;
//         public double rewardExponent;

//         public UpgradeRuntimeState(BeginningPlanetData masterData)
//         {
//             this.data = masterData;
//             this.level = (int)masterData.Level;
//             this.progress = 0;
//             this.ascensionCount = 0;
//             this.rewardExponent = 0;
//         }

//         public double GetCurrentCost() => data.Base_Cost * Math.Pow(data.Cost_Multiplier, level);
//     }
// }