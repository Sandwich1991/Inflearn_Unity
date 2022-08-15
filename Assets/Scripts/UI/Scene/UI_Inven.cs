using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPannel,
    }

    public override void init()
    {
        base.init();
        
        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPannel = Get<GameObject>((int)GameObjects.GridPannel);
        
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);
        
        // 실제 인벤토리 정보를 참고해서
        for (int i = 0; i < 8; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: gridPannel.transform).gameObject;
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"집행검{i + 1}번");
        }
        
        
    }
}