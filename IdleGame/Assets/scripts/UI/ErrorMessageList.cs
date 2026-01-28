using System.Collections.Generic;
using UnityEngine;

namespace MTProject.CosmicIdle.UI
{
    /// <summary>
    /// エラーの種類を定義
    /// </summary>
    public enum Error
    {
        Unknown,
        SaveDataNotFound,
        InvalidCredentials,
        InvalidInput
    }

    /// <summary>
    /// エラー表示用の拡張メソッド
    /// </summary>
    public static class ErrorExtensions
    {
        private static readonly Dictionary<Error, string> Messages = new Dictionary<Error, string>
        {
            { Error.Unknown, "未知のエラー" },
            { Error.SaveDataNotFound, "セーブデータがありません。" },
            { Error.InvalidCredentials, "名前もしくはパスワードが違います。" },
            { Error.InvalidInput, "名前もしくはパスワードが空白です。" }
        };

        // UI表示用のコールバック
        private static System.Action<string> onShow;

        /// <summary>
        /// UI側で一度だけ登録する
        /// </summary>
        public static void Init(System.Action<string> action) => onShow = action;

        /// <summary>
        /// メインの呼び出しメソッド
        /// 使用例: Error.SaveDataNotFound.Show();
        /// </summary>
        public static void Show(this Error error)
        {
            if (!Messages.TryGetValue(error, out string msg)) msg = "Error!";
            
            Debug.LogWarning(msg);
            onShow?.Invoke(msg);
        }
    }
}