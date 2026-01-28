using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTProject.CosmicIdle.UI
{
        
    [Serializable]
    public class MenuList
    {
        [SerializeField]
        public GameObject MenuObject;
        [SerializeField]
        public Button Button;
        [SerializeField]
        public Vector3 Obj_Position;
    }
    public class MainMenuChange : MonoBehaviour
    {
        [SerializeField]
        private List<MenuList> menuList = new List<MenuList>();
        [SerializeField]
        private GameObject DisPlayPosition;
        private MenuList tmp;

        void Start()
        {
        foreach (var entry in menuList)
        {
            entry.Button.onClick.AddListener(() => ChangeMenu(entry));
            entry.Obj_Position = entry.MenuObject.transform.localPosition;
        }
        menuList[0].MenuObject.transform.localPosition = DisPlayPosition.transform.localPosition;
        tmp = menuList[0];
        }

        private void ChangeMenu(MenuList selectedEntry)
        {
            Debug.Log($"{selectedEntry.Button.name}が押された");
            selectedEntry.MenuObject.transform.localPosition = DisPlayPosition.transform.localPosition;
            Debug.Log($"現在のtemp{tmp.MenuObject.name}");
            if ( tmp != selectedEntry )
            {
                tmp.MenuObject.transform.localPosition = tmp.Obj_Position;
                tmp = selectedEntry;
            }
        }
    }
}
