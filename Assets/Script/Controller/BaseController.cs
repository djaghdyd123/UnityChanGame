using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{


    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    [SerializeField]
    protected GameObject _lockTarget;

    public Define.WorldObject WorldObject { get; protected set; } = Define.WorldObject.Unknown;
    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator _anim = gameObject.GetComponent<Animator>();

            switch (_state)
            {
                case Define.State.Idle:
                    _anim.CrossFade("WAIT",0.1f);
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
                    break;
                case Define.State.Run_B:
                    _anim.CrossFade("WALK00_B", 0.1f);
                    break;
                case Define.State.Run_L:
                    _anim.CrossFade("WALK00_L", 0.1f);
                    break;
                case Define.State.Run_R:
                    _anim.CrossFade("WALK00_R", 0.1f);
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {

        switch (State)
        {
            case Define.State.Idle:
                Idle();
                break;
            case Define.State.Run:
                Run();
                break;
            case Define.State.Run_B:
                Run();
                break;
            case Define.State.Run_R:
                Run();
                break;
            case Define.State.Run_L:
                Run();
                break;
            case Define.State.Die:
                Die();
                break;
            case Define.State.Attack:
                Skill();
                break;
            case Define.State.Jump:
                Jump();
                break;

        }

    }

    public abstract void Init();
    protected virtual void Idle() { }
    protected virtual void Run() { }
    protected virtual void Die() { }
    protected virtual void Skill() { }
    protected virtual void Jump() { }

}
