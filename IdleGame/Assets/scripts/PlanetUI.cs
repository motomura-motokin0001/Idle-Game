using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetUI : MonoBehaviour
{
    [SerializeField] private int planetIndex; 
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    [Header("Ascension UI")]
    [SerializeField] private Button ascensionButton; // 惑星個別のアセンションボタン
    [SerializeField] private TextMeshProUGUI ascensionTargetText; // 「目標Lv: 100」などの表示用

    [Header("Debug")]
    [SerializeField]
    private string D_targetLevel;

    void Start()
    {
        //購入ボタンのイベント登録
        buyButton.onClick.AddListener(() => InfinitySystem.instance.BuyUpgrade(planetIndex));

        // アセンションボタンのイベント登録
        if (ascensionButton != null)
        {
            ascensionButton.onClick.AddListener(() => InfinitySystem.instance.Ascension(planetIndex));
        }
    }

    void Update()
    {
        if (InfinitySystem.instance == null || planetIndex >= InfinitySystem.instance.URTS.Count) return;
        
        var state = InfinitySystem.instance.URTS[planetIndex];

        // ゲージと基本情報の更新

        nameText.text = state.data.Planet_Name; 
        
        // アセンション回数（ランク）も表示に含めると分かりやすい
        levelText.text = $"Lv: {state.level} (Rank: {state.ascensionCount})";
        
        double cost = state.GetCurrentCost();
        costText.text = "Cost: " + NumberFormatter.FormatNumber(cost,InfinitySystem.instance.notationType);

        // 購入ボタンの有効化判定
        buyButton.interactable = InfinitySystem.instance.CurrentScore >= cost;
        if (state.Base_Speed >= 10.0)
        {
            fillImage.fillAmount = 1.0f;
            
            return;
        }
        fillImage.fillAmount = (float)state.progress;

        // //個別アセンションの判定
        UpdateAscensionUI(state);
    }

    private void UpdateAscensionUI(UpgradeRuntimeStatus state)
    {
        if (ascensionButton == null) return;

        int targetLevel = InfinitySystem.instance.GetAscensionGoal(planetIndex);
        
        // 目標レベルに達しているかチェック
        bool canAscend = state.level >= targetLevel;
        
        ascensionButton.gameObject.SetActive(canAscend);
        
        // if (ascensionTargetText != null)
        // {
        //     ascensionTargetText.text = $"Goal: Lv {targetLevel}";
        // }
    }
}