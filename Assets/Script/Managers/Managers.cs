using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    
    static Managers GetInstance() {Init(); return s_instance; }

    #region Contents
    GameManagerEX _game = new GameManagerEX();
    public static GameManagerEX Game { get { return GetInstance()._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resources = new ResourceManager();
    SceneManagerEX _scene = new SceneManagerEX();
    SoundManager _sound = new SoundManager();
    UIManagers _ui = new UIManagers();
 
    public static DataManager Data { get { return GetInstance()._data; } }
    public static InputManager Input { get { return GetInstance()._input; } }
    public static PoolManager Pool { get { return GetInstance()._pool; } }
    public static ResourceManager Resources { get { return GetInstance()._resources; } }
    public static SceneManagerEX Scene { get { return GetInstance()._scene; } }
    public static SoundManager Sound { get { return GetInstance()._sound; } }
    public static UIManagers UI { get {  return GetInstance()._ui; } }

    #endregion
    // 상단의 SubManager(프로퍼티)를 외부에 실행할 경우 s_instance(객체)안의 _필드를 가져오는 방식.

    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    void LateUpdate()
    {
        _input.OnLateUpdate();
    }


    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");   // s_instance가 참조할 하나의 객체 생성
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        s_instance._sound.Clear();
        Input.Clear();
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
    }
}