using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Description : UI_Popup
{

    enum Text1
    {
        Description,
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Text1));
    }

    public void WriteText(string text)
    {
        GetTextMeshProUGUI((int)Text1.Description).text = text;
    }
}
