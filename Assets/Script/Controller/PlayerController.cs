using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{

    PlayerStat _stat;


    int _mask = (1 << (int)Define.layer.Monster | (1 << (int)Define.layer.Ground));

    public override void Init()
    {
        WorldObject = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard; // Input에 이벤트가 발생시 OnKeyboard 함수 실행 (이벤트등록)
        Managers.Input.MousAction -= OnMouseEvent;
        Managers.Input.MousAction += OnMouseEvent;

        if(gameObject.GetComponentInChildren<UI_HpBar>() == null)
        Managers.UI.MakeWordSpaceUI<UI_HpBar>(transform); 

    }


    #region
    //GameObject (plaer)
    // transform
    // playerController (*)


    // Local -> World
    // transform.TransformDirection

    //World -> Local

    // transform.ReverseTransformDirection

    // 절대값 회전 
    // transform.eulerAngles = new Vector3(0.0f, 1.0f, 0.0f);

    // 델타 회전
    //  transform.Rotate(new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f)) ;

    // quaternion 회전
    // quaternion.LookRotation 은 월드좌표 기
    #endregion
    [SerializeField]
    Vector3 dir;
    protected override void Run()
    {

            if (_lockTarget != null)
            {
                _destPos = _lockTarget.transform.position;
                float distance = (_destPos - transform.position).magnitude;
                if (distance <= 1)
                {
                    State = Define.State.Attack;
                    return;
                }
            }

            //방향을 먼저 구하자
            dir = _destPos - transform.position;
            dir.y = 0;
            //오차 범위 허용
            if (dir.magnitude < 0.1f)
            {
                State = Define.State.Idle;
            }
            else
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
                {
                    if (Input.GetMouseButton(0) == false)
                        State = Define.State.Idle;
                    return;
                }
                float _moveDist = Mathf.Clamp(Time.deltaTime * _stat.MoveSpeed, 0, dir.magnitude);
                transform.position += dir.normalized * _moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

            }
 
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == (int)Define.layer.Ground)
        {
            State = Define.State.Idle;
            dir = Vector3.zero;
        }
        //if(collision.gameObject.layer == (int)Define.layer.Monster)
        //{
        //    Rigidbody rg = gameObject.GetComponent<Rigidbody>();
        //    rg.isKinematic = true;
        //}
    }

    float _jumpPower = 10.0f;
    [SerializeField]
    bool _isJumping = false;
    protected override void Jump()
    {

        transform.position += dir.normalized * Time.deltaTime * _stat.MoveSpeed;
        if (!_isJumping)
            return;

        Rigidbody rg = gameObject.GetComponent<Rigidbody>();
        rg.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        _isJumping = false;
    }

    protected override void Skill()
    {
        if(_lockTarget != null)
        {
            dir = _lockTarget.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }
    void OnLand()
    {
        Debug.Log("Land!");
        
    }
    void OnHitEvent()
    {

        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if(_stopSkill)
        {
            State = Define.State.Idle;

        }
        else
        {
            State = Define.State.Attack;
        }
        
    }

  
    void OnKeyboard()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
        }

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S )|| Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            State = Define.State.Idle;
        }
    }

    bool _stopSkill = false;

    void OnMouseEvent(Define.MousEvent evt)
    {
        switch(State)
        {
            case Define.State.Idle:
                onMouseEvent_IdleRun(evt);
                break;
            case Define.State.Run:
                onMouseEvent_IdleRun(evt);
                break;
            case Define.State.Attack:
                {
                    if (evt == Define.MousEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    void onMouseEvent_IdleRun(Define.MousEvent evt)
    {
        #region
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 MousPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dir = MousPos - Camera.main.transform.position;

            dir = dir.normalized;

            Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red , 1.0f);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
            {
                Debug.Log($"RayCast {hit.collider.gameObject.name}!");
            }
        }
        */
        #endregion

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MousEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                       
                        _destPos = hit.point;
                        _destPos.y = 0;
                        State = Define.State.Run;
                        _stopSkill = false;
                        if (hit.collider.gameObject.layer == (int)Define.layer.Monster)
                        {
                            _lockTarget = hit.collider.gameObject;
                        }
                        else if (hit.collider.gameObject.layer == (int)Define.layer.Ground)
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;

            case Define.MousEvent.Pressed:
                {
                    if (_lockTarget == null && raycastHit) _destPos = hit.point;
                }
                break;
            case Define.MousEvent.Clicked:
                break;
            case Define.MousEvent.PointerUp:
                _stopSkill = true;
                break;
            case Define.MousEvent.PointerDownRight:
                State = Define.State.Jump;
                _isJumping = true;
                break;
        }

    }
}