using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using MTProject.CosmicIdle.UI;

namespace MTProject.CosmicIdle.Save
{
    public class SaveLoad : MonoBehaviour
    {
    //private AccountUI accountUI = new AccountUI();
    private QuickSaveSettings Default_saveSettings;
    public DefaultData data = new DefaultData();
    public DataUI dataUI;

    void Awake()
    {

        #if UNITY_EDITOR
        QuickSaveGlobalSettings.StorageLocation = Application.dataPath + "/Save";
        #else
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        #endif

        Default_saveSettings = new QuickSaveSettings
        {
            SecurityMode = SecurityMode.Aes,
            Password = "MT_05021593", // 内部の暗号化キー
            CompressionMode = CompressionMode.Gzip
        };
        Load(data.Player_Name,data.Password);
    }

    public void Save(string name,string password)
    {
        string fileName = "User_" + name;
        bool hasData = false;


        try
        {
        var reader = QuickSaveReader.Create(fileName, Default_saveSettings);
        reader.Read<DefaultData>("AllData");
        hasData = true;
        }
        catch
        {
        hasData = false;
        }


        if (hasData)
        {
        Debug.Log(name + " さんの既存データが見つかりました。");
        }
        else
        {
        Debug.LogWarning(name + " さんのデータが見つかりませんでした。新規作成をします。");
        }
        // 2. 書き込み処理
        QuickSaveWriter writer = QuickSaveWriter.Create(fileName, Default_saveSettings);
        data.Player_Name = name;
        
        writer.Write("AllData", data);
        writer.Commit();

        Debug.Log("保存完了: " + fileName);
    }

    public void Load(string name,string password)
    {
        string fileName = "User_" + name;
        if (FileAccess.Exists(fileName))
        {
            if (data.Player_Name == name && data.Password == password)
            {
                QuickSaveReader reader = QuickSaveReader.Create(fileName, Default_saveSettings);
                data = reader.Read<DefaultData>("AllData");
                Debug.Log("読み込み完了");
                dataUI.UI(data);
            }
            else
            {
                Error.InvalidCredentials.Show();
            }
        }
        else
        {
            Error.SaveDataNotFound.Show();
        }
    }
}
}
