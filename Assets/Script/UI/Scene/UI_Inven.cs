using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach(Transform child in gridPanel.transform)
        {
            Managers.Resources.Destroy(child.gameObject);
        }

        // 실제 인벤토리 정보를 참고해서

        // prefab에 있는 UI_Inven_Item을 Grid 하위에 생성

        for(int i = 0; i <8;i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;
           

            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
           invenItem.SetInfo("블랙큐브");

        }
    }
}
