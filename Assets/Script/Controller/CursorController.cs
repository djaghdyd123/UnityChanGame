using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _handIcon;
    CursorType _cursorType = CursorType.None;

    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    int _mask = (1 << (int)Define.layer.Monster | (1 << (int)Define.layer.Ground));
  
    void Start()
    {
        _attackIcon = Managers.Resources.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resources.Load<Texture2D>("Textures/Cursor/Hand");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {

            if (hit.collider.gameObject.layer == (int)Define.layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.ForceSoftware);
                    _cursorType = CursorType.Attack;
                }

            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.ForceSoftware);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
