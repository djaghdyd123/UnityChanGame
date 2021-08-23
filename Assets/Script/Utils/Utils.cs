using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component; 
    }

    //GameObject 는 Component가 될수 없어 transform 으로 대체 하여 메소드 작성 transform == GameObject
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }


    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if(recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform.name == name || string.IsNullOrEmpty(name))
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T components in go.GetComponentsInChildren<T>())
            {
                if (components.name == name || string.IsNullOrEmpty(name))
                    return components;
            }
        }

        return null;
    }

    public static Vector3 RotateYAxis(Vector3 vect, float degree)
    {
        float rad = Mathf.Deg2Rad * degree;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        Vector3 newVector = new Vector3(vect.x * cos - vect.z * sin, vect.y, vect.z * cos + vect.x * sin);
        return newVector;
    }

    public static float GetYAngleBetweenVectors(Vector3 a, Vector3 b)
    {
        float dotProduct = Vector3.Dot(new Vector3(a.x, 0, a.z), new Vector3(b.x, 0, b.z));
        float  Mags= Vector3.Magnitude(new Vector3(a.x, 0, a.z)) * Vector3.Magnitude(new Vector3(b.x, 0, b.z));

        if(a.x < 0)
        return Mathf.Rad2Deg * Mathf.Acos(dotProduct / Mathf.Abs(Mags));
        else
            return -Mathf.Rad2Deg * Mathf.Acos(dotProduct / Mathf.Abs(Mags));


    }
} 
