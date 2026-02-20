using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UpgradeRuntimeStatus
{
    public BeginningPlanetData data;            // 設計図
    
    // --- 実行時ステータス ---
    public int level;                           // レベル（購入数）
    public int ascensionCount;                  // アセンション回数
    public double progress;                     // 進捗 (0.0 ～ 1.0)
    
    // --- 計算によって決まる値 (RecalculateStatsでキャッシュ) ---
    public double CurrentScoreIncrease;         // 報酬（1周あたり）
    public double CurrentSpeed;                 // スピード（1秒あたりの回転数 RPS）
    public double CurrentCost;                  // 次のレベルのコスト
    
    public UpgradeRuntimeStatus(BeginningPlanetData masterData)
    {
        this.data = masterData;
        this.level = 0; 
        this.ascensionCount = 0;
        this.progress = 0;
        RecalculateStats();
    }

    // ステータスの再計算処理
    public void RecalculateStats()
    {
        // 1. コスト計算: 指数関数的に増加
        CurrentCost = data.Base_Cost * Math.Pow(data.Cost_Multiplier, level);

        // 2. 報酬計算: アセンションによって「基礎報酬」が強化される
        // e308を目指すため、アセンション1回につき報酬を100倍にする
        double ascensionBonus = Math.Pow(100.0, ascensionCount);
        CurrentScoreIncrease = data.Base_Reward * ascensionBonus;

        // 3. スピード計算: レベルアップ（購入）によって回転速度が上がる
        // 例: 基礎スピード + (レベル * 0.1) 
        // 0.1 RPS (10秒に1回) が 10レベルで 1.1 RPS (約0.9秒に1回) になる
        double speedBonusPerLevel = 0.1; 
        CurrentSpeed = data.Base_Speed + (level * speedBonusPerLevel);
    }
}

public class InfinitySystem : MonoBehaviour
{
    public static InfinitySystem instance;

    [SerializeField]
    private List<BeginningPlanetData> BPD; 

    public List<UpgradeRuntimeStatus> URTS = new List<UpgradeRuntimeStatus>();

    public double CurrentScore;
    
    [Header("Ascension Settings")]
    public int baseAscensionGoal = 100;         // 最初のアセンションに必要なレベル
    public int levelIncrementPerAscension = 25; // 次のアセンションまでの必要レベル増分

    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI SCORE_Text; 

    public NotationType notationType;

    public TextMeshProUGUI Timer;
	[SerializeField]
	private int minute;
	[SerializeField]
	private float seconds;
	//　前のUpdateの時の秒数
	private float oldSeconds;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }

        InitializeGame();
    }

    void InitializeGame()
    {
        URTS.Clear();
        foreach (var data in BPD)
        {
            var status = new UpgradeRuntimeStatus(data);
            if (data.Level > 0) 
            {
                status.level = (int)data.Level;
                status.RecalculateStats();
            }
            URTS.Add(status);
        }

        if (CurrentScore <= 0 && URTS.Count > 0)
        {
            CurrentScore = URTS[0].CurrentCost;
        }
    }

    public UpgradeRuntimeStatus GetStatus(int index)
    {
        if (index >= 0 && index < URTS.Count) return URTS[index];
        return null;
    }

    void Update()
    {
        if (SCORE_Text != null)
        {
            SCORE_Text.SetText($"{NumberFormatter.FormatNumber(CurrentScore, notationType)}");
        }

        if (CurrentScore >= double.MaxValue) 
        {
            SCORE_Text.SetText("INFINITY");
        }
        else
        {
            seconds += Time.deltaTime;
            if(seconds >= 60f) 
            {
                minute++;
                seconds = seconds - 60;
            }
            //　値が変わった時だけテキストUIを更新
            if((int)seconds != (int)oldSeconds) 
            {
                Timer.text = minute.ToString("00") + ":" + ((int) seconds).ToString ("00");
            }
            oldSeconds = seconds;
        }

        foreach (var status in URTS)
        {
            if (status.level <= 0) continue;

            // 回転進捗の更新
            status.progress += status.CurrentSpeed * Time.deltaTime;

            if (status.progress >= 1.0)
            {
                double cycles = Math.Floor(status.progress);
                CurrentScore += cycles * status.CurrentScoreIncrease;
                status.progress -= cycles;
            }
        }
    }

    // 購入処理：スピードが上がる
    public void BuyUpgrade(int index)
    {
        var status = GetStatus(index);
        if (status == null) return;

        if (CurrentScore >= status.CurrentCost)
        {
            CurrentScore -= status.CurrentCost;
            status.level++;
            status.RecalculateStats();
            Debug.Log($"{status.data.Planet_Name} Level Up! Speed: {status.CurrentSpeed:F2} RPS");
        }
    }

    // アセンション：報酬倍率が上がる
    public void Ascension(int index)
    {
        var status = GetStatus(index);
        if (status == null) return;

        int goal = GetAscensionGoal(index);

        if (status.level >= goal)
        {
            status.ascensionCount++;
            
            // アセンション後のリセット処理
            status.level = 0; // スピードはリセットされるが報酬倍率が跳ね上がる
            status.progress = 0;
            status.RecalculateStats();

            Debug.Log($"{status.data.Planet_Name} Ascended! New Reward: {status.CurrentScoreIncrease:E2}");
        }
    }

    // 他スクリプトからも参照される判定用メソッド
    public int GetAscensionGoal(int index)
    {
        var status = GetStatus(index);
        if (status == null) return baseAscensionGoal;

        // アセンション回数に応じて必要レベルを上げる
        return baseAscensionGoal + (status.ascensionCount * levelIncrementPerAscension);
    }
}