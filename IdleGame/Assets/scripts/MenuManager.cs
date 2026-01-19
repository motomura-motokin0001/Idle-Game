using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuList
{
    public string _menuName;
    public GameObject _menuObject;
    public Button _menuButton;
}

public class MenuManager : MonoBehaviour
{
    public MenuList[] _menus;
    private List<Vector3> _menuPositions = new List<Vector3>();
    void Start()
    {
        foreach (MenuList menu in _menus)
        {
            _menuPositions.Add(menu._menuObject.transform.position);
            menu._menuButton.onClick.AddListener(() => OpenMenu(menu._menuName));
            
        }
        OpenMenu(_menus[0]._menuName); // Open the first menu by default
    }
    public void OpenMenu(string menuName)//ボタンが押されたら今screen内あるものを定位置に戻し、押されたメニューだけを中央に持ってくる
    {
        for (int i = 0; i < _menus.Length; i++)
        {
            if (_menus[i]._menuName == menuName)
            {
                _menus[i]._menuObject.transform.position = new Vector3(0, 0, 0); // Center position
            }
            else
            {
                _menus[i]._menuObject.transform.position = _menuPositions[i]; // Original position
            }
        }
    }
}
