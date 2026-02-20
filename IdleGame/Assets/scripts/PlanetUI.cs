using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class PlanetUI : MonoBehaviour
{
    [SerializeField] private int planetIndex;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    [Header("Ascension UI")]
    [SerializeField] private Button ascensionButton;
    [SerializeField] private TextMeshProUGUI ascensionTargetText;

    [Header("Localization")]
    // テーブル内のキーをインスペクターで指定（コードの初期値はバックアップ用）
    public LocalizedString costLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "COST_KEY" };
    public LocalizedString levelLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "LEVEL_KEY" };
    public LocalizedString rankLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "RANK_KEY" };

    private string currentCostLabel = "Cost";
    private string currentLevelLabel = "Lv";
    private string currentRankLabel = "Rank";

    void Start()
    {
        buyButton.onClick.AddListener(() => InfinitySystem.instance.BuyUpgrade(planetIndex));

        // 翻訳テキストが更新された時のイベント登録
        costLabel.StringChanged += (value) => currentCostLabel = string.IsNullOrEmpty(value) ? "Cost" : value;
        levelLabel.StringChanged += (value) => currentLevelLabel = string.IsNullOrEmpty(value) ? "Lv" : value;
        rankLabel.StringChanged += (value) => currentRankLabel = string.IsNullOrEmpty(value) ? "Rank" : value;

        if (ascensionButton != null)
        {
            ascensionButton.onClick.AddListener(() => InfinitySystem.instance.Ascension(planetIndex));
        }
    }

    void Update()
    {
        if (InfinitySystem.instance == null || planetIndex >= InfinitySystem.instance.URTS.Count) return;

        var state = InfinitySystem.instance.URTS[planetIndex];

        // 1. レベルとランクの表示更新
        levelText.text = $"{currentLevelLabel}: {state.level} ({currentRankLabel}: {state.ascensionCount})";

        // 2. コストの表示更新
        double cost = state.CurrentCost;
        costText.text = $"{currentCostLabel}: {NumberFormatter.FormatNumber(cost, InfinitySystem.instance.notationType)}";

        // 購入ボタンの有効化判定
        buyButton.interactable = InfinitySystem.instance.CurrentScore >= cost;
        
        fillImage.fillAmount = (float)state.progress;

        UpdateAscensionUI(state);
    }

    private void UpdateAscensionUI(UpgradeRuntimeStatus state)
    {
        if (ascensionButton == null) return;

        int targetLevel = InfinitySystem.instance.GetAscensionGoal(planetIndex);
        bool canAscend = state.level >= targetLevel;
        
        ascensionButton.gameObject.SetActive(canAscend);

    }
}