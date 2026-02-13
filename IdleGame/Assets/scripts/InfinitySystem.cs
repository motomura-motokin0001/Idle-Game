using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UpgradeRuntimeStatus
{
    public BeginningPlanetData data;            // 設計図 (ScriptableObject等)
    public int level;                           // 購入数
    public double progress;                     // 進捗 (0.0 ～ 1.0)
    public int ascensionCount;                  // アセンションの数
    public double Base_Score_Increase;          // 現在のスコア増加量
    public double Base_Speed;                   // 回転速度
    public double Base_Cost;                    // 次のレベルのコスト
    public double Cost_Multiplier;              // コストの増加倍率

    public UpgradeRuntimeStatus(BeginningPlanetData masterData)
    {
        this.data = masterData;
        this.level = (int)masterData.Level;
        this.progress = 0;
        this.ascensionCount = 0;
        this.Base_Score_Increase = masterData.Base_Reward;
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

private UpgradeRuntimeStatus Getstatus(int index)
{
    if (index < 0 || index >= URTS.Count) return null;
    return URTS[index];
}

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

private UpgradeRuntimeStatus GetStatus(int index)
{
    if (index >= 0 && index < URTS.Count)
    {
        return URTS[index];
    }
    return null;
}

    void Update()
    {
        // スコア表示の更新
        if (SCORE_Text != null)
        {
            SCORE_Text.SetText(NumberFormatter.FormatNumber(CurrentScore, notationType));
        }

        // 各惑星の進捗更新
        foreach (var status in URTS)
        {
            UpdatestatusProgress(status);
        }

        if(CurrentScore >= double.MaxValue)
        {
            //TODO 最大値に達成した際の処理
        }
    }

    private void UpdatestatusProgress(UpgradeRuntimeStatus status)//#Y 回転速度処理
    {
        if (status.level > 0)
        {
            status.progress += status.Base_Speed *status.level* Time.deltaTime;

            if (status.progress >= 1.0)
            {
                // 進捗が1.0を超えたら報酬を加算
                double reward = status.Base_Score_Increase;
                CurrentScore += reward;

                status.progress %= 1.0; // 余りを次に持ち越す
            }
        }
    }

    public void BuyUpgrade(int index)//#Y 
    {
        if (index < 0 || index >= URTS.Count) return;

    var status = GetStatus(index);
    if (status == null) return; // 統一された安全策
        double cost = status.Base_Cost;

        if (CurrentScore >= cost)
        {
            // 購入処理
            CurrentScore -= cost; // ここで正常に引かれるようになります
            
            status.level++;
            // 例：購入時に報酬（生産量）を強化する場合
            status.Base_Speed += status.Base_Speed; 
            Debug.Log($"Index[{index}] 購入成功。残りスコア: {CurrentScore} 次回コスト: {status.Base_Cost}");
        }
        else
        {
            Debug.Log($"スコア不足: 必要 {cost} / 所持 {CurrentScore}");
        }
    }

    public void Ascension(int index)//再構築 処理
    {
        var status = GetStatus(index);
        if (status == null) return; // 統一された安全策
        int goal = GetAscensionGoal(index);

        if (status.level <= goal)
        {
            status.ascensionCount++;
            status.level = (int)5;
            status.progress = 0;
            status.Base_Cost = status.Base_Cost * Math.Pow(status.Cost_Multiplier + (status.level * 0.01), status.level);
            Debug.Log($"{status.data.Planet_Name} Ascended!");
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

}//TODO アセンション際のコストのリセットを実装