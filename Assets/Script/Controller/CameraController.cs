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

    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {
        
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
                transform.position = _player.transform.position + _delta.normalized * dist;

            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            } 
            
        }
        
    }
        
    // 외부에서 카메라 각도를 변경하고 싶을땧
    public void SetQuarterView(Vector3 delta)
    {
        _delta = delta;
        _mode = Define.CameraMode.QuarterView;
    }
}

