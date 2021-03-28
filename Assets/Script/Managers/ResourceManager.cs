using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    public T Load<T>(string path) where T : Object
    {
        //1 original 이미 메모리에 들고 있으면 바로 사용 (Load x)
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        

        GameObject original = Load<GameObject>($"prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Faild to load original : {path}");
            return null;
        }


        //2 이미 Root폴더에 대기(풀링)된 애가 있다면 Pop하여 사용

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;
         
        GameObject go = Object.Instantiate<GameObject>(original, parent);

        // delete "(Clone)" from original object
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);

        return go;
        
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 풀링이 필요하면 풀링매니저한테 위탁
        Poolable poolable = go.GetComponent<Poolable>();

        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
