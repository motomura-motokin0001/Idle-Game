using DG.Tweening;
using MTProject.CosmicIdle.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MTProject.CosmicIdle.UI
{
    public enum EnterList
    {
        Create,
        Load
    }

    public class AccountUI : MonoBehaviour
    {   
        [SerializeField]
        private SaveLoad saveLoad ;
        [SerializeField]
        private GameObject PupUp;
        [SerializeField]
        private TMP_InputField NameTextBox;
        [SerializeField]
        private TMP_InputField PasswordTextBox;
        [SerializeField]
        private Button EnterButton;
        [SerializeField]
        private Button[] CancelButton;
        [SerializeField]
        private TextMeshProUGUI Message;
        [SerializeField]
        private EnterList enterList;


        void Awake()
        {
            Debug.Log("設定開始");
            ErrorExtensions.Init(msg => Message.text = msg);
            Initialization();
            EnterButton.onClick.AddListener(Enter);
            foreach (var button in CancelButton)
            {
                button.onClick.AddListener(Cancel);
            }
        }

        public void CreateAccountPupUp()
        {
            //PupUp.transform.DOScale(Vector3.one,1f).SetEase(Ease.OutBack);//TODOもしかしたらアニメーション無くすかも
        }

        public void Login()
        {
            
        }
        public void Enter()
        {
            Debug.Log("Enter");
            if (!string.IsNullOrEmpty(NameTextBox.text) && !string.IsNullOrEmpty(PasswordTextBox.text))
            {
                var name = NameTextBox.text.Trim();
                var password = PasswordTextBox.text.Trim();
                
                switch (enterList)
                {
                    case EnterList.Create:
                    saveLoad.Save(name,password);
                    break;
                    case EnterList.Load:
                    saveLoad.Load(name,password);
                    break;
                }
                Debug.Log("ロードを試みます。");
                
            }
            else
            {
                Error.InvalidInput.Show();
            }
        }
        public void Cancel()
        {
            Initialization();
        }

        void Initialization()
        {
            //PupUp.transform.localScale = Vector3.zero;
            NameTextBox.text = "";
            PasswordTextBox.text = "";
            Message.SetText("");
        }

    }
}


