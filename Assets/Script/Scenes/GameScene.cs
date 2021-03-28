using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        Managers.UI.ShowSceneUI<UI_Inven>("UI_Inven");
        SceneType = Define.Scene.Game;

        Dictionary<int,Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Characters/UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        GameObject go = new GameObject { name = "Spawningpool" };
        MonsterGenerator mg = go.GetOrAddComponent<MonsterGenerator>();
        mg.SetKeepMonsterCount(1);
        

    } 

    public override void Clear()
    {
        
    }


}
 