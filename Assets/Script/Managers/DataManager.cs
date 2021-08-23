using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface ILoader<Key,Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // 일련의 리스트로 이루어진 Json Data를 딕셔너리화 해서 Manager에서 물고 있는다.
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();


    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    // ILoader 인터페이스를 갖고 있는 Loader (StatData / 리스트를 들고있고 그 리스트를 딕셔너리로 만드는 클래스)
    // Data에 Json을 넣어줌.
    
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        Loader Data = JsonUtility.FromJson<Loader>(textAsset.text);
        return Data;
    }

}
