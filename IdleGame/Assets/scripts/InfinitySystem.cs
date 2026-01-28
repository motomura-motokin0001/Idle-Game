using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UpgradeRuntimeStatus
{
    public BeginningPlanetData data; // 設計図 (ScriptableObject等)
    public int level;               // 購入数
    public double progress;         // 進捗 (0.0 ～ 1.0)
    public int ascensionCount;      // アセンションの数
    public double Base_Reward;      // 現在の報酬量
    public double Base_Speed;       // 回転速度
    public double Base_Cost;        // 次のレベルのコスト
    public double Cost_Multiplier;  // コストの増加倍率

    public UpgradeRuntimeStatus(BeginningPlanetData masterData)
    {
        this.data = masterData;
        this.level = (int)masterData.Level;
        this.progress = 0;
        this.ascensionCount = 0;
        this.Base_Reward = masterData.Base_Reward;
        this.Base_Speed = masterData.Base_Speed;
        
        // --- 修正ポイント：マスターデータから値をコピーする ---
        this.Base_Cost = masterData.Base_Cost;
        this.Cost_Multiplier = masterData.Cost_Multiplier;
    }

    // 現在のコストを計算して返す（Base_Costを直接書き換える運用なら不要ですが、ミス防止に役立ちます）
    public double GetCurrentCost() => Base_Cost;
}

public class InfinitySystem : MonoBehaviour
{
    public static InfinitySystem instance;

    [SerializeField]
    private List<BeginningPlanetData> BPD; // 各アップグレードのマスターデータリスト

    public List<UpgradeRuntimeStatus> URTS = new List<UpgradeRuntimeStatus>();

    public double CurrentScore;
    public int baseAscensionGoal = 100; // 初期のアセンションに必要なレベル

    [SerializeField]
    private TextMeshProUGUI SCORE_Text; // 質量の合計値表示テキスト
    [SerializeField]
    private TextMeshProUGUI Multiplier_Text; // 質量の計算式表示テキスト
    public int levelIncrementPerAscension = 10; // アセンションをした際の次回のアセンション増加量


    public NotationType notationType;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 実行用リストの初期化
        URTS.Clear();
        foreach (var data in BPD)
        {
            URTS.Add(new UpgradeRuntimeStatus(data));
        }

        // 初回起動時の初期スコア設定（最初の惑星が買える分だけ配布）
        if (URTS.Count > 0 && CurrentScore <= 0)
        {
            CurrentScore = URTS[0].Base_Cost;
        }
    }

    void Update()
    {
        // スコア表示の更新
        if (SCORE_Text != null)
        {
            SCORE_Text.SetText(NumberFormatter.FormatNumber(CurrentScore, notationType));
        }

        // 各惑星の進捗更新
        foreach (var planet in URTS)
        {
            UpdatePlanetProgress(planet);
        }
    }


    private void UpdatePlanetProgress(UpgradeRuntimeStatus planet)
    {
        if (planet.level > 0)
        {
            planet.progress += planet.Base_Speed *planet.level* Time.deltaTime;

            if (planet.progress >= 1.0)
            {
                // 進捗が1.0を超えたら報酬を加算
                double reward = planet.Base_Reward;
                CurrentScore += reward;

                planet.progress %= 1.0; // 余りを次に持ち越す
                
                // 報酬が変わる仕組み（Revolution Idle風にレベルアップでBase_Rewardを増やす等）があるならここでも表示更新
                // UpdateMultiplierDisplay(); 
            }
        }
    }

    public void BuyUpgrade(int index)
    {
        if (index < 0 || index >= URTS.Count) return;

        var planet = URTS[index];
        double cost = planet.Base_Cost;

        if (CurrentScore >= cost)
        {
            // 購入処理
            CurrentScore -= cost; // ここで正常に引かれるようになります
            
            planet.level++;
            
            // 次回のコストを計算して更新
            planet.Base_Cost *= planet.Cost_Multiplier;

            // 例：購入時に報酬（生産量）を強化する場合
            planet.Base_Speed += planet.Base_Speed; 

            Debug.Log($"Index[{index}] 購入成功。残りスコア: {CurrentScore} 次回コスト: {planet.Base_Cost}");
        }
        else
        {
            Debug.Log($"スコア不足: 必要 {cost} / 所持 {CurrentScore}");
        }
    }

    public void PerformIndividualAscension(int index)
    {
        if (index < 0 || index >= URTS.Count) return;
        var planet = URTS[index];
        int goal = GetAscensionGoal(index);

        if (planet.level >= goal)
        {
            planet.ascensionCount++;
            planet.level = (int)planet.data.Level;
            planet.progress = 0;
            Debug.Log($"{planet.data.Planet_Name} Ascended!");
        }
    }
        public int GetAscensionGoal(int index) // #Yアセンション目標レベルの取得
    {
        if (index < 0 || index >= URTS.Count)
        {
            return baseAscensionGoal;
        }

        return baseAscensionGoal + (URTS[index].ascensionCount * levelIncrementPerAscension);
    }
}