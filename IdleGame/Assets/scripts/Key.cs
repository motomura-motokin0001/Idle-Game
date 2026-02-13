using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// Unity Input Systemを使用して、キー入力中に連続的なアクションを実行するコンポーネント
/// </summary>
public class Key : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("連打のアクション（例: Spaceキーなど）")]
    public InputAction inputAction;

    [Tooltip("入力の間隔（秒単位）")]
    public float interval = 0.1f;

    [Tooltip("トグルモードにするか（一度押すと開始、もう一度押すと停止）")]
    public bool isToggleMode = false;

    private Coroutine firingCoroutine;
    private bool isToggled = false;

    private void OnEnable()
    {
        // InputActionの初期化とコールバック登録
        inputAction.Enable();
        
        // 押された時と離された時のイベント
        inputAction.started += OnInputStarted;
        inputAction.canceled += OnInputCanceled;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.started -= OnInputStarted;
        inputAction.canceled -= OnInputCanceled;
        StopFiring();
    }

    private void OnInputStarted(InputAction.CallbackContext context)
    {
        if (isToggleMode)
        {
            isToggled = !isToggled;
            if (isToggled) StartFiring();
            else StopFiring();
        }
        else
        {
            StartFiring();
        }
    }

    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        if (!isToggleMode)
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireRoutine());
        }
    }

    private void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    /// <summary>
    /// 実際の連打処理を行うコルーチン
    /// </summary>
    private IEnumerator FireRoutine()
    {
        while (true)
        {
            PerformAction();
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// ここに連打したい具体的な処理を記述します
    /// </summary>
    private void PerformAction()
    {
        // デバッグ用ログ。ここに攻撃処理やジャンプ処理などを記述してください。
        Debug.Log($"Action Performed at: {Time.time}");
    }
}