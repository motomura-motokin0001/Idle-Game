using TMPro;
using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using MTProject.CosmicIdle.Save;

namespace MTProject.CosmicIdle.UI
{
    public class DataUI : MonoBehaviour
    {
        [SerializeField]
        private SaveLoad saveLoad ;
        [SerializeField]
        private TextMeshProUGUI Name;
        [SerializeField]
        private TextMeshProUGUI Pass;
        [SerializeField]
        private TextMeshProUGUI Score;

        public void UI(DefaultData data)
        {
            Name.SetText(data.Player_Name);
            Score.SetText(data.TotalMass.ToString());
        }
    }
}

