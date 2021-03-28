using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    // 현제 Scene을 외부에서 불러오기

    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // Enum SceneType을 인자로 LoadScene 사용

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
