using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    [SerializeField]
    float yDegree;
    protected PlayerStat _stat;

    public GameObject cam = null;

    int _mask = (1 << (int)Define.layer.Monster | (1 << (int)Define.layer.Ground));

    Define.CameraMode _cameraMode = Define.CameraMode.QuarterView;
    public Define.CameraMode Mode
    {
        get { return _cameraMode; }
        set
        {
            _cameraMode = value;
        }
    }
    public override void Init()
    {
        WorldObject = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard; // Input에 이벤트가 발생시 OnKeyboard 함수 실행 (이벤트등록)
        Managers.Input.MousAction -= OnMouseEvent;
        Managers.Input.MousAction += OnMouseEvent;



        // HP bar
        //if(gameObject.GetComponentInChildren<UI_HpBar>() == null)
        //Managers.UI.MakeWordSpaceUI<UI_HpBar>(transform); 

    }

    public override Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator _anim = gameObject.GetComponent<Animator>();

            switch (_state)
            {
                case Define.State.Idle:
                    _anim.CrossFade("WAIT", 0.1f);
                    break;

                case Define.State.Die:
                    break;
                case Define.State.Attack:
                    _anim.CrossFade("ATTACK", 0.1f);
                    break;
                case Define.State.Jump:
                    _anim.CrossFade("JUMP", 0.1f);
                    break;
                case Define.State.Run:
                    _anim.CrossFade("RUN", 0.1f);
                    _stat.MoveSpeed = 5.0f;
                    break;
                case Define.State.Run_FL:
                    _anim.CrossFade("RUN", 0.1f);
                    _stat.MoveSpeed = 3.5f;
                    break;
                case Define.State.Run_FR:
                    _anim.CrossFade("RUN", 0.1f);
                    _stat.MoveSpeed = 3.5f;
                    break;
                case Define.State.Run_B:
                    _anim.CrossFade("WALK00_B", 0.1f);
                    _stat.MoveSpeed = 2.0f;
                    break;
                case Define.State.Run_L:
                    _anim.CrossFade("WALK00_L", 0.1f);
                    _stat.MoveSpeed = 2.0f;
                    break;
                case Define.State.Run_R:
                    _anim.CrossFade("WALK00_R", 0.1f);
                    _stat.MoveSpeed = 2.0f;
                    break;
            }
        }
    }
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
                // 장애물 맞닥뜨릴때 더이상 transform x  마우스 때기전까지 계속 이동모션
                if (Input.GetMouseButton(0) == false)
                {
                    State = Define.State.Idle;
                    return;
                }
            }
            float _moveDist = Mathf.Clamp(Time.deltaTime * _stat.MoveSpeed, 0, dir.magnitude);
            transform.position += dir.normalized * _moveDist;


            // 목표지점을 기준으로 캐릭터 방향 회전
            switch (State)
            {
                case Define.State.Run:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
                    break;
                case Define.State.Run_L:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateVector(dir, 270)), 10f * Time.deltaTime);
                    break;
                case Define.State.Run_R:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateVector(dir, 90)), 10f * Time.deltaTime);
                    break;
                case Define.State.Run_B:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-dir), 10f * Time.deltaTime);
                    break;
                case Define.State.Run_FL:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateVector(dir, 315)), 10f * Time.deltaTime);
                    break;
                case Define.State.Run_FR:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateVector(dir, 45)), 10f * Time.deltaTime);
                    break;
            }
        }
        
           
 
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
        switch (Mode)
        {
            case Define.CameraMode.QuarterView:
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
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    State = Define.State.Idle;
                }
                break;

            case Define.CameraMode.ShoulderView:

                // 1.카메라방향을 기준으로 이동한다.
                // 2.캐릭터회전은 이동지점을 기준으로 한다.
                // 3.카메라위치는 캐릭터회전과 같은속도로 하여 2번을 offset. (바꾸지 않으면, 카메라 방향이 캐릭터 회전에 따라 계속 바뀌므로)
                // 3-2. Y 축 회전만 위치를 보정.
                // Error  Vector3.Slerp 에서 Y 회전이 조금씩 일어남.
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                {
                    if (_state != Define.State.Run_FL)
                    {
                        State = Define.State.Run_FL;
                    }
                    //1
                    _destPos = transform.position + Utils.RotateYAxis(camDir(), 45);
                    //3
                    cam.GetComponent<CameraController>().MakeDefaultDelta();
                }
                else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                {
                    if (_state != Define.State.Run_FR)
                    {
                        State = Define.State.Run_FR;
                    }
                    _destPos = transform.position + Utils.RotateYAxis(camDir(), 315);
                    cam.GetComponent<CameraController>().MakeDefaultDelta();
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (_state != Define.State.Run)
                    {
                        State = Define.State.Run;
                    }
               
                    _destPos = transform.position +  camDir();
                    cam.GetComponent<CameraController>().MakeDefaultDelta();

                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (_state != Define.State.Run_B)
                    {
                        State = Define.State.Run_B;
                    }
                    _destPos = transform.position -  camDir();

                    cam.GetComponent<CameraController>().MakeDefaultDelta();
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    if (_state != Define.State.Run_L)
                    {
                        State = Define.State.Run_L;
                    }

                    _destPos = transform.position + rotateVector(camDir(), 90);
                    cam.GetComponent<CameraController>().MakeDefaultDelta();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (_state != Define.State.Run_R)
                    {
                        State = Define.State.Run_R;
                    }
                    _destPos = transform.position + rotateVector(camDir(), 270);
                    cam.GetComponent<CameraController>().MakeDefaultDelta();
                }
                else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    State = Define.State.Idle;
                }
                break;
        }  
    }

    Vector3 rotateVector(Vector3 vect, float deg)
    {
        float rad = Mathf.Deg2Rad*deg;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        Vector3 newVector = new Vector3(vect.x * cos - vect.z * sin, vect.y, vect.z * cos + vect.x * sin);
        return newVector;
    }

    Vector3 camDir()
    {
        return (gameObject.transform.position-cam.transform.position).normalized;
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
            
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
            

        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        
        switch (evt)
        {
            case Define.MousEvent.PointerDown:
                switch (_cameraMode)
                {
                    case Define.CameraMode.QuarterView:
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
                        break;
                    case Define.CameraMode.ShoulderView:
                            break;
                }
                break;
            case Define.MousEvent.Pressed:
                    
                switch (_cameraMode)
                {
                    case Define.CameraMode.QuarterView:
                    if (_lockTarget == null && raycastHit) _destPos = hit.point;
                        break;
                    case Define.CameraMode.ShoulderView:
                        break;
                }         
                break;
            case Define.MousEvent.Clicked:
                switch (_cameraMode)
            {
                case Define.CameraMode.QuarterView:
                       
                    break;
                case Define.CameraMode.ShoulderView:
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
                    break;
            }
                break;
            case Define.MousEvent.PointerUp:
                _stopSkill = true;
                break;
            case Define.MousEvent.PointerDownRight:

                break;
        }
            //lastTick = currentTick;
    }
}