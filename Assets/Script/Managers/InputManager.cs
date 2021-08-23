using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction;
    public Action<Define.MousEvent> MousAction;


    bool _pressed = false;
    bool _pressedRight = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();
        if (MousAction != null)
        {
            // 누르는동안 pressed 이벤트 발생 하며 놓는 순간 Clicked 이벤트 발생
            if (Input.GetMouseButton(0))
            {
                if(_pressed == false)
                {
                    MousAction.Invoke(Define.MousEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MousAction.Invoke(Define.MousEvent.Pressed);
                _pressed = true;
            }
            else // 때는 순간 클릭인지 아닌지 판단.
            {
                if (_pressed)
                {
                    if (Time.time <= _pressedTime + 0.2f) 
                        MousAction.Invoke(Define.MousEvent.Clicked);
                    MousAction.Invoke(Define.MousEvent.PointerUp);

                }
                _pressed = false;
                _pressedTime = 0;
            }

 
        }
    }
    public void OnLateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (MousAction != null)
        {
            if (Input.GetMouseButton(1))
            {
                if (_pressedRight == false)
                {
                    // 우클릭 한번 했을때.
                }
                MousAction.Invoke(Define.MousEvent.PressedRight);
                _pressedRight = true;
            }
            else
            {
                if (_pressedRight)
                {
                    MousAction.Invoke(Define.MousEvent.PointerUpRight);
                }
                _pressedRight = false;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MousAction = null;
    }
}
