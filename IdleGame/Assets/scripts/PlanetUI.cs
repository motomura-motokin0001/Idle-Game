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
    // テーブル内の「Cost」などのラベル用
    public LocalizedString costLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "COST_KEY" };
    // テーブル内の「Level」や「Rank」などのラベル用
    public LocalizedString levelLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "LEVEL_KEY" };
    public LocalizedString RankLabel = new LocalizedString { TableReference = "MyTextTable", TableEntryReference = "RANK_KEY" };

    private string currentCostLabel = "Cost"; // 翻訳されたラベルを保持
    private string currentLevelLabel = "Lv";   // 翻訳されたラベルを保持
    private string currentRankLabel =  "Rank";

    void Start()
    {
        buyButton.onClick.AddListener(() => InfinitySystem.instance.BuyUpgrade(planetIndex));

        // 言語が切り替わったときにラベルを更新するイベントを登録
        costLabel.StringChanged += (value) => currentCostLabel = value;
        levelLabel.StringChanged += (value) => currentLevelLabel = value;

        if (ascensionButton != null)
        {
            ascensionButton.onClick.AddListener(() => InfinitySystem.instance.Ascension(planetIndex));
        }
    }

    void Update()
    {
        if (InfinitySystem.instance == null || planetIndex >= InfinitySystem.instance.URTS.Count) return;

        var state = InfinitySystem.instance.URTS[planetIndex];

        // 1. レベルとランクの表示更新 (多言語ラベル + 数値)
        // 例: "Lv: 10 (Rank: 1)" -> "レベル: 10 (ランク: 1)" 
        levelText.text = $"{currentLevelLabel}: {state.level} ({currentRankLabel}: {state.ascensionCount})";

        // 2. コストの表示更新 (多言語ラベル + フォーマット済み数値)
        // 例: "Cost: 1.2k" -> "コスト: 1.2k"
        double cost = state.GetCurrentCost();
        costText.text = $"{currentCostLabel}: {NumberFormatter.FormatNumber(cost, InfinitySystem.instance.notationType)}";

        // 購入ボタンの有効化判定
        buyButton.interactable = InfinitySystem.instance.CurrentScore >= cost;
        
        if (state.Base_Speed >= 10.0)
        {
            fillImage.fillAmount = 1.0f;
            return;
        }
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