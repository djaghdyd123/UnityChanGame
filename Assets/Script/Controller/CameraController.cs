using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] // private이지만 unity 모드에서는 값을 넣어줄수 있음.
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    GameObject _player = null;

    bool _pressed = false;

    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {
        Managers.Input.MousAction -= OnMouseDrag;
        Managers.Input.MousAction += OnMouseDrag;

    }

    // Key 이동 업데이트가 우선 시행되고 카메라가 따라가는 방식으로해야 카메라의 이동이 자연스럽다.
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
                return;

            RaycastHit hit;

            // 플레이어에서 카메라 방향 벡터로 Raycast하여 장애물이 있는지 판단
            if(Physics.Raycast(_player.transform.position,_delta, out hit, _delta.magnitude,LayerMask.GetMask("Block")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist + new Vector3(0.0f,1.0f,0.0f);
                // TODO 카메라 각도도 조금 바꾸면 좋을것같다

            }
            else
            { 
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            } 
            
        }

        if (_mode == Define.CameraMode.ShoulderView)
        {
            if (_player.IsValid() == false)
                return;

            // local 이므로 부모가 바라보는 방향이 축의 기준이됨.
            transform.localPosition = _delta;

            // 드래그 풀면 천천히 카메라가 디폴드값이 됨.
            if(!_pressed)
            _delta = Vector3.Slerp(_delta, new Vector3(0.0f, 2.0f, -2.5f), 0.1f);
            Vector3 dir = _player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir + new Vector3(0,1.5f,0));
        }

    }
        
    // 외부에서 카메라 각도를 변경하고 싶을땧
    public void SetQuarterView(Vector3 delta)
    {
        _delta = delta;
        _mode = Define.CameraMode.QuarterView;
    }

    public void SetShoulderView()
    {
        gameObject.transform.SetParent(_player.transform);
        _delta = new Vector3(0.0f, 2.0f, -2.5f);
        _mode = Define.CameraMode.ShoulderView;
        _player.GetOrAddComponent<PlayerController>().Mode = Define.CameraMode.ShoulderView;
    }


    public void OnMouseDrag(Define.MousEvent evt)
    {
        switch(evt)
        {
            case Define.MousEvent.PressedRight:
                _pressed = true;
                _delta = Utils.RotateYAxis(_delta, Input.GetAxis("Mouse X") * Mathf.Rad2Deg / 3);
                break;
            case Define.MousEvent.PointerUpRight:
                _pressed = false;
                break;
        }
            
    }

}

