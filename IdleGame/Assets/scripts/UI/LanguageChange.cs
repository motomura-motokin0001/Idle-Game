using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
namespace MTProject.CosmicIdle.UI
{
    public class LanguageChange : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        void Start()
        {
            // 初期化を待ってから現在の設定を反映
            StartCoroutine(InitializeDropdown());
        }

        private IEnumerator InitializeDropdown()
        {
            // LocalizationSettingsの初期化完了を待つ
            yield return LocalizationSettings.InitializationOperation;

            // 利用可能な言語リストを取得
            var options = new List<TMP_Dropdown.OptionData>();
            int currentLocaleIndex = 0;
            var locales = LocalizationSettings.AvailableLocales.Locales;

            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    currentLocaleIndex = i;
                
                string name = locale.Identifier.CultureInfo != null 
                    ? locale.Identifier.CultureInfo.NativeName 
                    : locale.ToString();

                options.Add(new TMP_Dropdown.OptionData(name));
            }

            dropdown.options = options;
            dropdown.value = currentLocaleIndex;

            // 値が変わった時のリスナーを登録
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int index)
        {
            // 言語を切り替える
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}
