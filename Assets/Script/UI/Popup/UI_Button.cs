using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
   
    enum Button1
    {
        Button,
    }

    enum Text1
    {
        ButtonText,
        ScoreText
    }

    enum GameObject1
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Button1));
        Bind<Text>(typeof(Text1));
        Bind<Image>(typeof(Images));

        GetButton((int)Button1.Button).gameObject.AddUIEvent(OnButtonClicked);
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

    }
    int _score = 0;

    void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Text1.ScoreText).text = $"Score : {_score}";
    }

}
 