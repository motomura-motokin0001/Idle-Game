// using DG.Tweening;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Threading.Tasks; // Taskを使用するために必要

// public class PasswordChecker : MonoBehaviour
// {
//     [SerializeField] private GameObject passwordCheckWindow;
//     [SerializeField] private TMP_InputField passwordTextBox; // TMPを使う場合はこちら
//     [SerializeField] private Button enterButton;
//     [SerializeField] private Button cancelButton;
//     [SerializeField] private TextMeshProUGUI messageBox;

//     private TaskCompletionSource<string> _passwordTaskSource;

//     void Awake()
//     {
//         // 初期状態は非表示（スケール0）
//         passwordCheckWindow.transform.localScale = Vector3.zero;
        
//         enterButton.onClick.AddListener(OnEnterClicked);
//         cancelButton.onClick.AddListener(OnCancelClicked);
//     }

//     // 外部からこのメソッドを呼んで、パスワード入力を「待つ」
//     public async Task<string> WaitPasswordInputAsync()
//     {
//         // メッセージをリセットしてウィンドウを表示
//         messageBox.SetText("");
//         passwordTextBox.text = "";
//         passwordCheckWindow.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

//         // 入力完了を待機するための準備
//         _passwordTaskSource = new TaskCompletionSource<string>();

//         // ボタンが押されるまでここで待機する
//         string result = await _passwordTaskSource.Task;

//         return result;
//     }

//     private void OnEnterClicked()
//     {
//         // 待機中のTaskに入力された文字列を渡して完了させる
//         _passwordTaskSource?.TrySetResult(passwordTextBox.text);
//     }

//     private void OnCancelClicked()
//     {
//         // キャンセル時は空文字、あるいは null を返す
//         _passwordTaskSource?.TrySetResult(null);
//         passwordCheckWindow.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
//     }

//     public void ShowFailedMessage()
//     {
//         messageBox.SetText("パスワードが違うようです。再度お試しください。");
//         // 揺らす演出（DOTween）を追加するとより良くなります
//         passwordCheckWindow.transform.DOShakePosition(0.5f, 10f);
//     }
// }