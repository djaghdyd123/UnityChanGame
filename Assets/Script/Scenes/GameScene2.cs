using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene2 : BaseScene
{
    protected override void Init()
    {
        base.Init();

        //Managers.UI.ShowPopupUI<UI_Button>();
        SceneType = Define.Scene.Game2;

        // Scene에 필요 데이터를 들고 있는 것 같다.
        //Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Characters/ShoulderViewController");
        //player.transform.position = new Vector3(-2.50f, 0f, 0f);

        //// Camera Setting
        {
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetShoulderView2();
        }
        // GameManager에서 오브젝트들을 들고 있다.


        Managers.Sound.Play("UnityChan/AiryBass", Define.Sound.Bgm);
       
    }

    public override void Clear()
    {

    }


}