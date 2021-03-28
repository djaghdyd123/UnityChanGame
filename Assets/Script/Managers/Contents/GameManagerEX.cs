using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEX
{
    // int <-> GameObject
    [SerializeField]
    GameObject _player;

    public Action<int> OnSpawnEvent;

    HashSet<GameObject> _monster = new HashSet<GameObject>();

    public GameObject GetPlayer() { return _player; } 

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent  = null)
    {
        GameObject go = Managers.Resources.Instantiate(path, parent);

        switch(type)
        {
            case Define.WorldObject.Monster:
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                _monster.Add(go);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
         
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;
        return bc.WorldObject;
        
    }

    public void Despawn(GameObject go)
    {

        Define.WorldObject type = GetWorldObjectType(go);
        switch (type)
        {
            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
            case Define.WorldObject.Monster:
                if (_monster.Contains(go))
                {
                    _monster.Remove(go);
                    OnSpawnEvent.Invoke(-1);
                }  
                break;
        }

        Managers.Resources.Destroy(go);
    }
}
    
